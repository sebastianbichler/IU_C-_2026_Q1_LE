
---

# Hintergrundinformationen:

1. **Boxing-Visualisierung:** `ArrayList` nutzt intern ein `object[]`. Wenn ein `int` (Stack) dort
   hinein soll, muss die CLR eine "Box" auf dem Heap bauen. Das kostet Zeit und erzeugt Müll (GC-Last).
2. **Cache-Lokalität:** Bei der `struct`-Liste liegen die Daten wie Soldaten in einer Reihe. Die CPU lädt sie in "
   Cache-Lines" voraus. Bei Klassen-Listen muss die CPU für jedes Element erst die Adresse lesen und dann an eine ganz
   andere Stelle im RAM springen.
3. **LINQ & Performance:** Weisen Sie darauf hin, dass LINQ elegant ist, aber in extremen High-Performance-Loops der
   GAE-Engine (z.B. Kollisionsabfrage 60x pro Sekunde) eine einfache `for`-Schleife über ein Array oft besser ist, um
   Allokationen zu vermeiden.

