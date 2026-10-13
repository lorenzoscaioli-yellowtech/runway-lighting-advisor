# Requisiti — luminosità consigliata per circuito

Strumento dimostrativo. Configurazioni, soglie e nomi sono **inventati**: non riproducono
normative ICAO né impianti reali.

## Scopo

Dato lo stato di visibilità di un aeroporto, suggerire a quale gradino di luminosità portare
ciascun circuito luminoso di una pista.

## Dominio

- Ogni **circuito** (avvicinamento, soglia, bordo pista, asse pista, taxiway) è alimentato da un
  **regolatore a corrente costante (CCR)**.
- I regolatori hanno **5 gradini** (B1–B5).
- Una **configurazione** è l'insieme dei circuiti di una pista.

## Regole

1. **Di notte**, in base alla visibilità:

   | Visibilità | Gradino |
   |---|---|
   | ≥ 5000 m | B1 |
   | 1500 – 4999 m | B2 |
   | 800 – 1499 m | B3 |
   | 400 – 799 m | B4 |
   | < 400 m | B5 |

2. **Di giorno** si sale di un gradino, senza superare B5.
3. **Taxiway**: un gradino sotto la pista, senza scendere sotto B1.
4. Una visibilità negativa è rifiutata.
