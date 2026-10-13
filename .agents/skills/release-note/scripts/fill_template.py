#!/usr/bin/env python3
"""Fill the release-note .docx template with the content of a JSON file.

Standard library only. The agent writes the content (JSON); this script only
does the formatting, so the layout is identical on every release.

Usage:
    python3 fill_template.py <template.docx> <content.json> <output.docx>

JSON fields:
    version, date, summary, reviewer : strings
    features, fixes, notes           : lists of strings (one bullet each)
"""
import json
import re
import sys
import zipfile
from xml.sax.saxutils import escape

SCALARS = {"VERSIONE": "version", "DATA": "date", "SINTESI": "summary", "REVISORE": "reviewer"}
LISTS = {"NOVITA": "features", "CORREZIONI": "fixes", "NOTE": "notes"}
PARAGRAPH = re.compile(r"<w:p(?: [^>]*)?>.*?</w:p>", re.DOTALL)


def fill(xml: str, content: dict) -> str:
    for placeholder, key in LISTS.items():
        items = content.get(key) or ["Nessuna."]
        token = "{{%s}}" % placeholder

        def expand(match):
            paragraph = match.group(0)
            if token not in paragraph:
                return paragraph
            # Clone the bullet paragraph once per item, keeping its formatting.
            return "".join(paragraph.replace(token, escape(str(item))) for item in items)

        xml = PARAGRAPH.sub(expand, xml)

    for placeholder, key in SCALARS.items():
        if key not in content:
            raise SystemExit(f"Missing field in JSON: {key}")
        xml = xml.replace("{{%s}}" % placeholder, escape(str(content[key])))

    leftover = re.findall(r"\{\{[A-Z]+\}\}", xml)
    if leftover:
        raise SystemExit(f"Unfilled placeholders: {leftover}")
    return xml


def main() -> None:
    if len(sys.argv) != 4:
        raise SystemExit(__doc__)
    template, content_path, output = sys.argv[1:]
    with open(content_path, encoding="utf-8") as f:
        content = json.load(f)

    with zipfile.ZipFile(template) as src, zipfile.ZipFile(output, "w", zipfile.ZIP_DEFLATED) as dst:
        for item in src.infolist():
            data = src.read(item.filename)
            if item.filename in ("word/document.xml", "word/footer1.xml"):
                data = fill(data.decode("utf-8"), content).encode("utf-8")
            dst.writestr(item, data)
    print(f"Written {output}")


if __name__ == "__main__":
    main()
