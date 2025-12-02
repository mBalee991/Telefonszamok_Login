# Telefonszámok WPF Alkalmazás – README

Ez a dokumentum összefoglalja a *Telefonszámok* nevű WPF alkalmazás működését, az Entity Framework alapú adatkezelést, valamint a projekt felépítését. 
A leírás a tanár úr által megadott logikát és a Model First megközelítést követi.

---

## 📌 Projekt áttekintése
A program célja egy egyszerű, Model First Entity Framework alapokra épülő, WPF felülettel rendelkező alkalmazás, amely személyeket, helységeket és telefonszámokat kezel.

A fő funkciók:
- Helységek listázása
- Helységadatok módosítása
- Személyek és hozzájuk tartozó adatok listázása
- Telefonszámok összegyűjtése és megjelenítése
- EF adatbázis-kezelés, módosítások mentése

---

## 🗄 Az adatbázis szerkezete
A program három fő entitást kezel:

### **1. Helyseg**
- `Id` – elsődleges kulcs
- `IRSZ` – irányítószám
- `Nev` – település neve

### **2. Szemely**
- `Id` – elsődleges kulcs
- `Vezeteknev`
- `Utonev`
- `Lakcim`
- `enHelysegId` – külső kulcs a Helyseg táblához
- `Felhasznalonev`
- `JelszoHash`

### **3. Telefonszam**
- `Id` – elsődleges kulcs
- `Szam` – telefonszám
- `enSzemelyId` – külső kulcs a Szemely táblához

A kapcsolatok:
- Egy személy **egy helységhez** tartozik
- Egy személyhez **több telefonszám** is tartozhat

---

## 🧩 Fő panelok és funkciók

### **1. Helységek listázása (miHelyseg)**
- A DataGrid statikus (nem automatikus) oszlopokkal jeleníti meg a helységeket.
- Mezők: *Név*, *IRSZ*

### **2. Minden adat listázása (miMindenAdat)**
Betölti:
- személyek adatait
- helységnevet (navigációs property-n keresztül)
- telefonszámokat, vesszővel elválasztva

### **3. Helységadatok módosítása**
- A két ComboBox a meglévő helységek közül választ.
- A felhasználó bármelyik ComboBox-ban választ, a másik automatikusan követi.
- A kijelölt helység adatai megjelennek a szövegmezőkben.
- A **Módosított adatpár rögzítése** gomb *azonnal módosítja a kiválasztott helység objektumot* az EF memóriájában.
- A **Vissza** gomb elrejti a szerkesztő panelt, üríti a beviteli mezőket

## 🔄 Adatkezelés – EF logika – EF logika

### **Módosított adatpár rögzítése gomb**
- Kiválasztott helységet módosítja
- Csak EF memóriában (változáskövetés)
- Nem véglegesíti az adatbázisban

### **Mentés menüpont**
- `SaveChanges()` meghívása
- Minden EF-ben tárolt módosítást az adatbázisba ír
---

## 🔧 SQL séma
A projektben használt SQL séma Devart Entity Developer által generált, teljesen kompatibilis az EF modellel.

A táblák: `Helyseg`, `Szemely`, `Telefonszam`, megfelelő elsődleges és külső kulcsokkal. (Lásd: DatabaseScript.sql)

---

## 📁 Projekt felépítése
- **Telefonszamok (WPF)** – GUI réteg
- **Model** – EF entitások, Kontextus
- **Database** – SQL szerver adatbázis

---

## 🚀 Futási követelmények
- .NET Framework / .NET Core WPF környezet
- Entity Framework 6.x
- SQL Server 2019 vagy újabb

---

## 📌 Kapcsolódó anyag
- Johanyák Zsolt Csaba: *Vizuális programozás – gyakorlati jegyzet*, EF és WPF példák.
- https://johanyak.hu/segedlet/vp/Vizualis_programozas_gyakorlati_jegyzet_VS_2019.pdf
---

## Default Login
- Felhasználónév: admin
- Jelszó: admin

## Utolsó módosítás:
- 2025/12/02 - 22:28
