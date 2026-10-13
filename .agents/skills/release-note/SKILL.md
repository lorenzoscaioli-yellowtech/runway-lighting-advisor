---
name: release-note
description: Writes the Italian release note (.docx) of Runway Lighting Advisor from the conventional commits since the last release, using the company template. Use when asked for a release note, "nota di rilascio", or the document of a new version.
---

# Release note

Produce the release note of a new version as a Word file, from the company template.
The content comes from the commits; the layout comes from the template and must not change.

## Steps

1. **Find the commits of the release.** Last tag: `git describe --tags --abbrev=0`.
   Commits since then: `git log <last-tag>..HEAD --pretty=format:'%h %s%n%b'`.
   If there is no tag, use the whole history.
2. **Group them by conventional-commit type:**
   - `feat` → *Novità*
   - `fix` → *Correzioni*
   - anything that changes how the system is installed or configured (new configuration
     fields, behaviour that depends on the installed hardware) → *Note per l'installazione
     e la compatibilità*
   - `chore`, `ci`, `test`, `refactor`, `docs` → leave out, unless they change something a
     user can see.
3. **Write for the reader of the note: a system engineer, not a developer.** Italian, one
   sentence per bullet, say what changes for whoever configures or uses the system. No commit
   hashes, no class or method names. `summary` is two or three sentences.
4. **Write the content as JSON** in `release-notes/<version>.json`:
   `version`, `date` (DD/MM/YYYY), `summary`, `features`, `fixes`, `notes` (lists of strings),
   `reviewer` = `"da compilare"` — a person fills it after reading the note.
5. **Generate the .docx** with the script, never by hand:
   `python3 .agents/skills/release-note/scripts/fill_template.py .agents/skills/release-note/templates/nota-di-rilascio.docx release-notes/<version>.json release-notes/nota-di-rilascio-<version>.docx`
6. **Report** the file path and list anything you were unsure how to classify.

## Rules

- Do not invent features: every bullet must come from a commit.
- If the version number is not given, ask for it.
- The note is a draft until a person fills in the reviewer.
