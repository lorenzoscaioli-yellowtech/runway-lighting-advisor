# Requisiti — luminosità consigliata per circuito

Strumento dimostrativo. Configurazioni, soglie e nomi sono **inventati**: non riproducono
normative ICAO né impianti reali.

## Scopo

Dato lo stato di visibilità di un aeroporto, suggerire a quale gradino di luminosità portare
ciascun circuito luminoso di una pista.

## Dominio

- Ogni **circuito** (avvicinamento, soglia, bordo pista, asse pista, taxiway) è alimentato da un
  **regolatore a corrente costante (CCR)**.
- I regolatori più recenti hanno **5 gradini** (B1–B5); quelli più vecchi ne hanno **3** (B1–B3).
- Una **configurazione** è l'insieme dei circuiti di una pista. Un impianto può avere regolatori
  dei due tipi insieme.

## Regole

1. **Scala di riferimento a 5 gradini, di notte**, in base alla visibilità:

   | Visibilità | Gradino |
   |---|---|
   | ≥ 5000 m | B1 |
   | 1500 – 4999 m | B2 |
   | 800 – 1499 m | B3 |
   | 400 – 799 m | B4 |
   | < 400 m | B5 |

2. **Di giorno** si sale di un gradino, senza superare B5.
3. **Taxiway**: un gradino sotto la pista, senza scendere sotto B1.
4. **Regolatori a 3 gradini**: il gradino della scala di riferimento si riporta sulla scala del
   regolatore in proporzione, arrotondando per eccesso. Un regolatore a 3 gradini **non può mai
   ricevere un gradino oltre B3**.
5. Visibilità negativa e regolatori con un numero di gradini diverso da 3 o 5 sono rifiutati.
