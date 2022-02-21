using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace SystemKina
{
    class Program
    {
        public const string nazwaPlikuKino = "kino.txt";
        public const string nazwaPlikuUzytkownicy = "uzytkownicy.txt";
        public static List<Konto> listaUzytkownikow;

        public static int PodajInt(string sTekst, int iMin = int.MinValue, int iMax = int.MaxValue)
        {
            while (true)
            {
                Console.Write(sTekst);

                if (int.TryParse(Console.ReadLine(), out int iLiczba) == true)
                {
                    if (iLiczba >= iMin && iLiczba <= iMax)
                        return iLiczba;

                    else
                        Console.WriteLine("Podano nieprawidłowy przedział!");
                }
                else
                    Console.WriteLine("Podano nieprawidłową liczbę!");
            }
        }

        public struct Konto
        {
            public string imie;
            public string nazwisko;
            public string email;
            public string haslo;

            public string PodajEmaila()
            {
                while (true)
                {
                    Console.Write("Podaj email: ");
                    string email = Console.ReadLine();
                    string trimmedEmail = email.Trim();

                    if (trimmedEmail.EndsWith("."))
                    {
                        Console.WriteLine("Podano nieprawidłowy adres email! \nSpróbuj ponownie");
                        continue;
                    }
                    try
                    {
                        var addr = new System.Net.Mail.MailAddress(email);
                        
                        if (addr.Address == trimmedEmail)
                        {
                            return trimmedEmail;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Podano nieprawidłowy adres email! \nSpróbuj ponownie");
                    }
                }
            }

            public string PodajHaslo()
            {
                while (true)
                {
                    Console.Write("Podaj hasło (min. 8 znaków): ");
                    string haslo = Console.ReadLine().Trim();

                    if (haslo.Length >= 8) return haslo;
                    else
                    {
                        Console.WriteLine("Hasło jest za słabe! Spróbuj ponownie");
                    }
                }
            }

            public bool Rejestracja()
            {
                Console.Clear();
                Console.Write("Podaj imię: ");
                imie = Console.ReadLine();

                Console.Write("Podaj nazwisko: ");
                nazwisko = Console.ReadLine();

                // zrobić sprawdzenie poprawności maila i hasła
                string email = PodajEmaila();

                foreach (Konto konto in listaUzytkownikow)
                {
                    if (konto.email == email)
                    {
                        Console.WriteLine("ISTNIEJE JUŻ KONTO O PODANYM EMAILU !");
                        Console.WriteLine("\n-- naciśnij dowolny przycisk --");
                        Console.ReadKey();

                        return false;
                    }
                }
                this.email = email;

                haslo = PodajHaslo();

                return true;
            }

            public void Logowanie()
            {
                while (true)
                {
                    Console.Clear();
                    Console.Write("Podaj email: ");
                    string email = Console.ReadLine();
                    bool znalezionoKonto = false;

                    foreach (Konto szukaneKonto in listaUzytkownikow)
                    {
                        if (szukaneKonto.email == email)
                        {
                            this.email = email;
                            znalezionoKonto = true;

                            while (true)
                            {
                                Console.Write("Podaj hasło: ");
                                string haslo = Console.ReadLine();

                                if (szukaneKonto.haslo == haslo)
                                {
                                    this.haslo = haslo;
                                    imie = szukaneKonto.imie;
                                    nazwisko = szukaneKonto.nazwisko;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Podano błędne hasło!");
                                    Console.WriteLine("Kliknij ENTER, żeby spróbować ponownie!");

                                    if (Console.ReadKey().Key != ConsoleKey.Enter)
                                        break;
                                }
                            }

                            break;
                        }
                    }

                    if (znalezionoKonto) break;
                    else
                    {
                        Console.WriteLine("Nie znaleziono konta z podanym mailem!");
                        Console.WriteLine("Kliknij ESC, żeby wyjść!");

                        if (Console.ReadKey().Key == ConsoleKey.Escape)
                            break;
                    }
                }
            }
        }

        public struct Miejsce
        {
            public int rzad;
            public int miejsce;
            public string emailUzytkownika;

            public Miejsce(int rzad, int miejsce, string email)
            {
                this.rzad = rzad;
                this.miejsce = miejsce;
                emailUzytkownika = email;
            }
        }

        public struct Seans
        {
            public string tytul;
            public string poczatekSeansu;
            public string koniecSeansu;
            public int minWiek;
            public List<Miejsce> zajeteMiejsca;

            public void WyswietlSeans()
            {
                Console.WriteLine($"Tytuł filmu: {tytul}");
                Console.WriteLine($"Godzina rozpoczęcia się filmu: {poczatekSeansu}");
                Console.WriteLine($"Godzina zakończenia się filmu: {koniecSeansu}");
                Console.WriteLine($"Minimalny wiek: {minWiek}\n");
            }
        }

        public struct Sala
        {
            public int nrSali;
            public List<Seans> seanse;
            public int iloscRzedow;
            public int iloscMiejscWRzedzie;
            
            public Sala(int nrSali, int iloscRzedow, int iloscMiejscWRzedzie)
            {
                this.nrSali = nrSali;
                seanse = new List<Seans>();
                this.iloscRzedow = iloscRzedow;
                this.iloscMiejscWRzedzie = iloscMiejscWRzedzie;
            }

            public DateTime PodajGodzine1(ref Seans seans)
            {
                while (true)
                {
                    Console.Write("Podaj godzinę rozpoczęcia się filmu (w formacie hh:mm): ");
                    seans.poczatekSeansu = Console.ReadLine();

                    if (DateTime.TryParseExact(seans.poczatekSeansu, "HH:mm", null, DateTimeStyles.None, out DateTime poczatekSeansu))
                        return poczatekSeansu;

                    else
                        Console.WriteLine("Podano godzinę w nieprawidłowym formacie!");
                }
            }

            public DateTime PodajGodzine2(ref Seans seans, DateTime poczatekSeansu)
            {
                while (true)
                {
                    Console.Write("Podaj godzinę zakończenia się filmu (w formacie hh:mm): ");
                    seans.koniecSeansu = Console.ReadLine();

                    if (DateTime.TryParseExact(seans.koniecSeansu, "HH:mm", null, DateTimeStyles.None, out DateTime koniecSeansu))
                    {
                        if (koniecSeansu > poczatekSeansu)
                            return koniecSeansu;

                        else
                            Console.WriteLine("Zły przedział czasowy!");
                    }

                    else
                        Console.WriteLine("Podano godzinę w nieprawidłowym formacie!");
                }
            }

            public bool CzyBladCzasowy(List<Seans> seanse, DateTime poczatekDodawanegoSeansu, DateTime koniecDodawanegoSeansu, int zmienianyIndeks = -1)
            {
                for (int i = 0; i < seanse.Count; i++)
                {
                    if (i != zmienianyIndeks)
                    {
                        DateTime poczatekSeansu = DateTime.Parse(seanse[i].poczatekSeansu);
                        DateTime koniecSeansu = DateTime.Parse(seanse[i].koniecSeansu);

                        if (
                                (koniecDodawanegoSeansu > poczatekSeansu || koniecDodawanegoSeansu >= koniecSeansu) &&
                                (poczatekDodawanegoSeansu <= poczatekSeansu || poczatekDodawanegoSeansu < koniecSeansu)
                            )
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            public void DodajSeans()
            {
                Console.Clear();
                Seans dodawanySeans = new Seans();
                dodawanySeans.zajeteMiejsca = new List<Miejsce>();

                Console.Write("Podaj tytuł filmu: ");
                dodawanySeans.tytul = Console.ReadLine();

                DateTime poczatekDodawanegoSeansu = PodajGodzine1(ref dodawanySeans);
                DateTime koniecDodawanegoSeansu = PodajGodzine2(ref dodawanySeans, poczatekDodawanegoSeansu);
                
                int wiek = PodajInt("Podaj minimalny wiek: ", 1, 100);
                dodawanySeans.minWiek = wiek;

                if (CzyBladCzasowy(seanse, poczatekDodawanegoSeansu, koniecDodawanegoSeansu))
                    Console.WriteLine("Seanse nakładają się na siebie!");

                else
                    seanse.Add(dodawanySeans);

                WyswietlSeanse();
            }

            public void UsunSeans()
            {
                Console.Clear();
                WyswietlSeanse();
                int numerSeansu = PodajInt("Podaj numer seansu do usunięcia (wpisz 0, żeby wrócić): ", 0, seanse.Count);

                if (numerSeansu == 0)
                    return;

                for (int i = 0; i < seanse.Count; i++)
                {
                    if (numerSeansu == i + 1)
                    {
                        seanse.RemoveAt(i);
                    }
                }
            }

            public void ModyfikujSeans()
            {
                Console.Clear();
                WyswietlSeanse();
                int numerSeansu = PodajInt("Podaj numer seansu, który chcesz zmodyfikować (wpisz 0, żeby wrócić): ", 0, seanse.Count);

                if (numerSeansu == 0)
                    return;

                for (int i = 0; i < seanse.Count; i++)
                {
                    if (numerSeansu == i + 1)
                    {
                        DateTime poczatekSeansu, koniecSeansu;
                        Seans seans = seanse[i];

                        Console.Clear();
                        Console.WriteLine($"1. tytuł filmu: {seans.tytul}");
                        Console.WriteLine($"2. godziny trwania filmu: {seans.poczatekSeansu} - {seans.koniecSeansu}");
                        Console.WriteLine($"3. minimalny wiek: {seans.minWiek}");
                        int odp = PodajInt("Wybierz, którą właściwość chcesz zmienić (wpisz 0, żeby wrócić): ", 0, 3);
                        Console.WriteLine();

                        switch (odp)
                        {
                            case 1:
                                Console.Write("Podaj tytuł filmu: ");
                                seans.tytul = Console.ReadLine();
                                break;

                            case 2:
                                poczatekSeansu = PodajGodzine1(ref seans);
                                koniecSeansu = PodajGodzine2(ref seans, DateTime.Parse(seans.poczatekSeansu));

                                if (CzyBladCzasowy(seanse, poczatekSeansu, koniecSeansu, i))
                                {
                                    Console.WriteLine("Seanse nakładają się na siebie!");
                                    return;
                                }
                                break;

                            case 3:
                                int wiek = PodajInt("Podaj minimalny wiek: ", 1, 100);
                                seans.minWiek = wiek;
                                break;
                        }

                        seanse[i] = seans;
                    }
                }
            }

            public void WyswietlSeanse()
            {
                Console.WriteLine();
                for (int i = 0; i < seanse.Count; i++)
                {
                    Console.WriteLine(i + 1 + ".");
                    seanse[i].WyswietlSeans();
                }
            }

            // zwraca numer seansu wyświetlanej sali
            public int WyswietlSale(string emailUzytkownika = null)
            {
                Console.Clear();
                WyswietlSeanse();
                int nrSeansu = PodajInt("Podaj numer seansu, żeby wyświetlić plan kina (wpisz 0, żeby wrócić): ", 0, seanse.Count);

                if (nrSeansu == 0)
                    return 0;

                Seans seans = seanse[nrSeansu - 1];
                WyswietlPlanSali(seans, emailUzytkownika);

                return nrSeansu;
            }

            public void WyswietlPlanSali(Seans seans, string emailUzytkownika)
            {
                Console.Clear();
                for (int i = 0; i < iloscRzedow; i++)
                {
                    Console.Write(i + 1 + "\t");

                    for (int j = 0; j < iloscMiejscWRzedzie; j++)
                    {
                        int index = seans.zajeteMiejsca.FindIndex(zajeteMiejsce => zajeteMiejsce.rzad == i + 1 && zajeteMiejsce.miejsce == j + 1);

                        if (index >= 0)
                        {
                            Console.ForegroundColor = seans.zajeteMiejsca[index].emailUzytkownika == emailUzytkownika ? ConsoleColor.Blue : ConsoleColor.Red;
                            Console.Write(j >= 9 ? "[X ] " : "[X] ");
                            Console.ResetColor();
                        }
                        else
                            Console.Write($"[{j + 1}] ");
                        
                    }
                    Console.WriteLine();
                }
            }
        }

        public struct Kino
        {
            public List<Sala> sale;

            public void DodajSale()
            {
                int nrSali;

                if (sale.Count > 0)
                    nrSali = sale[sale.Count - 1].nrSali + 1;

                else
                    nrSali = 1;

                int iloscRzedow = PodajInt("Podaj ilość rzędów w sali <1; 100>: ", 1, 100);
                int iloscMiejsc = PodajInt("Podaj ilość miejsc w rzędzie w sali <1; 99>: ", 1, 99);

                Sala nowaSala = new Sala(nrSali, iloscRzedow, iloscMiejsc);
                sale.Add(nowaSala);
                WyswietlWszystkieSeansy();
                Console.WriteLine($"Dodano salę nr {nowaSala.nrSali}");
            }

            public void UsunSale()
            {
                int numer = PodajInt("Podaj nr sali do usunięcia: ", 1);

                for (int i = 0; i < sale.Count; i++)
                {
                    if (numer == sale[i].nrSali)
                    {
                        sale.RemoveAt(i);
                        WyswietlWszystkieSeansy();
                        return;
                    }
                }

                Console.WriteLine("Nie znaleziono sali o tym numerze!");
            }

            public void WyswietlSale()
            {
                int nrSali = PodajInt("Podaj numer sali, której plan chcesz wyświetlić: ", 1);

                foreach(Sala sala in sale)
                {
                    if (nrSali == sala.nrSali)
                    {
                        sala.WyswietlSale();
                    }
                }
            }

            public void WyswietlWszystkieSeansy()
            {
                Console.Clear();

                foreach (Sala sala in sale)
                {
                    Console.WriteLine($"sala nr {sala.nrSali}");
                    sala.WyswietlSeanse();
                    Console.WriteLine("-----------------------------");
                }
            }

            public void DodajSeans()
            {
                int numer = PodajInt("Podaj nr sali seansu: ", 1);

                foreach (Sala sala in sale)
                {
                    if (numer == sala.nrSali)
                    {
                        sala.DodajSeans();
                        return;
                    }
                }

                Console.WriteLine("Nie znaleziono sali o tym numerze!");
            }

            public void ModyfikujSeans()
            {
                int numer = PodajInt("Podaj nr sali seansu: ", 1);

                foreach (Sala sala in sale)
                {
                    if (numer == sala.nrSali)
                    {
                        sala.ModyfikujSeans();
                        return;
                    }
                }

                Console.WriteLine("Nie znaleziono sali o tym numerze!");
            }

            public void UsunSeans()
            {
                int numer = PodajInt("Podaj nr sali seansu: ", 1);

                foreach (Sala sala in sale)
                {
                    if (numer == sala.nrSali)
                    {
                        sala.UsunSeans();
                        return;
                    }
                }

                Console.WriteLine("Nie znaleziono sali o tym numerze!");
            }

            // moduł rezerwacji
            public Konto Rejestracja()
            {
                Konto noweKonto = new Konto();
                if (noweKonto.Rejestracja())
                {
                    listaUzytkownikow.Add(noweKonto);
                    ZapiszDane();
                }

                return noweKonto;
            }

            public Konto Logowanie()
            {
                Konto kontoDoSprawdzenia = new Konto();
                kontoDoSprawdzenia.Logowanie();

                return kontoDoSprawdzenia;
            }

            public void WyswietlSeanseDoWyboru(string emailUzytkownika)
            {
                WyswietlWszystkieSeansy();
                Console.WriteLine();
                int nrSali = PodajInt("Podaj numer sali do rezerwacji: ", 1);
                int nrSeansu = sale[nrSali - 1].WyswietlSale(emailUzytkownika);
                if (nrSeansu == 0)
                    return;

                WyborMiejsca(sale[nrSali - 1], sale[nrSali - 1].seanse[nrSeansu - 1], emailUzytkownika);
            }

            public void WyszukajPoTytule(string emailUzytkownika)
            {
                Console.Clear();
                Console.Write("Wyszukaj seans: ");
                string szukanaFraza = Console.ReadLine();
                Console.WriteLine();

                int i = 0;
                foreach (Sala sala in sale)
                {
                    foreach (Seans seans in sala.seanse)
                    {
                        if (seans.tytul.Contains(szukanaFraza, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine(++i);
                            seans.WyswietlSeans();
                        }
                    }
                }

                if (i == 0)
                {
                    Console.WriteLine("Nie znaleziono pasujących tytułów!");
                    return;
                }

                int nrSeansu = PodajInt("Podaj numer seansu do rezerwacji (podaj 0, żeby wyjść): ", 0);
                if (nrSeansu == 0) return;

                i = 0;
                foreach (Sala sala in sale)
                {
                    foreach (Seans seans in sala.seanse)
                    {
                        if (seans.tytul.Contains(szukanaFraza, StringComparison.OrdinalIgnoreCase))
                        {
                            i++;
                            if (i == nrSeansu)
                            {
                                sala.WyswietlPlanSali(seans, emailUzytkownika);
                                WyborMiejsca(sala, seans, emailUzytkownika);
                            }
                        }
                    }
                }
            }

            public void WyborMiejsca(Sala sala, Seans seans, string emailUzytkownika)
            {
                //wybór miejsca
                while (true)
                {
                    int nrRzedu = PodajInt("\nPodaj numer rzędu do rezerwacji: ", 1, sala.iloscRzedow);
                    int nrMiejsca = PodajInt("Podaj numer miejsca do rezerwacji: ", 1, sala.iloscMiejscWRzedzie);
                    int index = seans.zajeteMiejsca.FindIndex(zajeteMiejsce => zajeteMiejsce.rzad == nrRzedu && zajeteMiejsce.miejsce == nrMiejsca);

                    if (index >= 0)
                    {
                        if (seans.zajeteMiejsca[index].emailUzytkownika == emailUzytkownika)
                            Console.WriteLine("To miejsce jest już zajęte przez Ciebie! Kliknij ENTER, żeby kontynuować!");

                        else
                            Console.WriteLine("To miejsce jest zajęte! Kliknij ENTER, żeby kontynuować!");

                        if (Console.ReadKey().Key != ConsoleKey.Enter)
                            return;
                    }
                    else
                    {
                        seans.zajeteMiejsca.Add(new Miejsce(nrRzedu, nrMiejsca, emailUzytkownika));
                        Console.WriteLine("ZAREZERWOWANO");
                        break;
                    }
                }
            }

            public void AnulujRezerwacje(string emailUzytkownika)
            {
                WyswietlWszystkieSeansy();
                int nrSali = PodajInt("Podaj numer sali do anulowania rezerwacji: ", 1);
                Sala sala = sale[nrSali - 1];
                int nrSeansu = sala.WyswietlSale(emailUzytkownika);
                Seans seans = sala.seanse[nrSeansu - 1];

                if (seans.zajeteMiejsca.Count == 0)
                {
                    Console.WriteLine("\nŻadne miejsce na ten seans nie jest zarezerwowane!");
                    return;
                }

                while (true)
                {
                    int nrRzedu = PodajInt("\nPodaj numer rzędu do anulowania rezerwacji: ", 1, sala.iloscRzedow);
                    int nrMiejsca = PodajInt("Podaj numer miejsca do anulowania rezerwacji: ", 1, sala.iloscMiejscWRzedzie);
                    Miejsce miejsce = new Miejsce(nrRzedu, nrMiejsca, emailUzytkownika);

                    if (seans.zajeteMiejsca.Contains(miejsce))
                    {
                        seans.zajeteMiejsca.Remove(miejsce);
                        Console.WriteLine($"ANULOWANO REZERWACJĘ MIEJSCA: rząd {nrRzedu}, miejsce {nrMiejsca}");
                        break;   
                    }
                    else
                    {
                        Console.WriteLine("To miejsce nie jest zarezerwowane przez Ciebie! Kliknij ENTER, żeby kontynuować!");

                        if (Console.ReadKey().Key != ConsoleKey.Enter)
                            break;
                    }
                }
            }

            public void ZapiszDane()
            {
                using StreamWriter zapisUzytkownikow = new StreamWriter(nazwaPlikuUzytkownicy);

                zapisUzytkownikow.WriteLine("[Uzytkownicy]");
                zapisUzytkownikow.WriteLine(listaUzytkownikow.Count);

                foreach (Konto konto in listaUzytkownikow)
                {
                    zapisUzytkownikow.WriteLine(konto.imie);
                    zapisUzytkownikow.WriteLine(konto.nazwisko);
                    zapisUzytkownikow.WriteLine(konto.email);
                    zapisUzytkownikow.WriteLine(konto.haslo);
                }

                using StreamWriter zapisDanych = new StreamWriter(nazwaPlikuKino);

                zapisDanych.WriteLine("[Kino]");
                zapisDanych.WriteLine(sale.Count);

                foreach (Sala sala in sale)
                {
                    zapisDanych.WriteLine("[Sala]");
                    zapisDanych.WriteLine(sala.nrSali);
                    zapisDanych.WriteLine(sala.iloscRzedow);
                    zapisDanych.WriteLine(sala.iloscMiejscWRzedzie);
                    zapisDanych.WriteLine(sala.seanse.Count);

                    foreach (Seans seans in sala.seanse)
                    {
                        zapisDanych.WriteLine("[Seans]");
                        zapisDanych.WriteLine(seans.tytul);
                        zapisDanych.WriteLine(seans.poczatekSeansu);
                        zapisDanych.WriteLine(seans.koniecSeansu);
                        zapisDanych.WriteLine(seans.minWiek);
                        zapisDanych.WriteLine("[Zajete miejsca na seans]");

                        if (seans.zajeteMiejsca != null)
                        {
                            zapisDanych.WriteLine(seans.zajeteMiejsca.Count);

                            foreach (Miejsce miejsce in seans.zajeteMiejsca)
                            {
                                zapisDanych.WriteLine($"{miejsce.rzad} {miejsce.miejsce} {miejsce.emailUzytkownika}");
                            }
                        }
                    }
                }
            }

            public void OdczytajDane()
            {
                if (File.Exists(nazwaPlikuUzytkownicy))
                {
                    using StreamReader odczytUzytkownikow = new StreamReader(nazwaPlikuUzytkownicy);

                    if (odczytUzytkownikow.ReadLine() == "[Uzytkownicy]")
                    {
                        int iloscUzytkownikow = int.Parse(odczytUzytkownikow.ReadLine());

                        for (int i = 0; i < iloscUzytkownikow; i++)
                        {
                            Konto konto = new Konto
                            {
                                imie = odczytUzytkownikow.ReadLine(),
                                nazwisko = odczytUzytkownikow.ReadLine(),
                                email = odczytUzytkownikow.ReadLine(),
                                haslo = odczytUzytkownikow.ReadLine()
                            };

                            listaUzytkownikow.Add(konto);
                        }
                    }
                }

                if (File.Exists(nazwaPlikuKino))
                {
                    using StreamReader odczytDanych = new StreamReader(nazwaPlikuKino);

                    if (odczytDanych.ReadLine() == "[Kino]")
                    {
                        int iloscSal = int.Parse(odczytDanych.ReadLine());

                        for (int i = 0; i < iloscSal; i++)
                        {
                            if (odczytDanych.ReadLine() == "[Sala]")
                            {
                                Sala sala = new Sala
                                {
                                    seanse = new List<Seans>(),
                                    nrSali = int.Parse(odczytDanych.ReadLine()),
                                    iloscRzedow = int.Parse(odczytDanych.ReadLine()),
                                    iloscMiejscWRzedzie = int.Parse(odczytDanych.ReadLine())
                                };

                                int iloscSeansow = int.Parse(odczytDanych.ReadLine());

                                for (int j = 0; j < iloscSeansow; j++)
                                {
                                    if (odczytDanych.ReadLine() == "[Seans]")
                                    {
                                        string tytul = odczytDanych.ReadLine();
                                        string poczatekSeansu = odczytDanych.ReadLine();
                                        string koniecSeansu = odczytDanych.ReadLine();
                                        int minWiek = int.Parse(odczytDanych.ReadLine());

                                        List<Miejsce> zajeteMiejsca = new List<Miejsce>();

                                        if (odczytDanych.ReadLine() == "[Zajete miejsca na seans]")
                                        {
                                            int iloscZajetychMiejsc = int.Parse(odczytDanych.ReadLine());

                                            for (int k = 0; k < iloscZajetychMiejsc; k++)
                                            {
                                                string[] pozycja = odczytDanych.ReadLine().Trim().Split(' ');
                                                int rzad = int.Parse(pozycja[0]);
                                                int miejsce = int.Parse(pozycja[1]);
                                                string email = pozycja[2];

                                                zajeteMiejsca.Add(new Miejsce(rzad, miejsce, email));
                                            }
                                        }

                                        Seans seans = new Seans
                                        {
                                            tytul = tytul,
                                            poczatekSeansu = poczatekSeansu,
                                            koniecSeansu = koniecSeansu,
                                            minWiek = minWiek,
                                            zajeteMiejsca = zajeteMiejsca
                                        };

                                        sala.seanse.Add(seans);
                                    }
                                }

                                sale.Add(sala);
                            }
                        }
                    }
                }
            }
        }

        public static void GlowneMenu()
        {
            listaUzytkownikow = new List<Konto>();
            Kino kino = new Kino();
            kino.sale = new List<Sala>();
            kino.OdczytajDane();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("0. Wyjście z programu");
                Console.WriteLine("1. Moduł zarządzania kinem");
                Console.WriteLine("2. Moduł rezerwacji");
                int iOdp = PodajInt("Wybierz moduł: ", 0, 2);

                switch (iOdp)
                {
                    case 0: return;
                    case 1: MenuZarzadzania(kino); break;
                    case 2: MenuRezerwacji(kino); break;
                }
            }
        }

        public static void MenuZarzadzania(Kino kino)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Wróć do wyboru modułu\n");
                Console.WriteLine("2. Dodaj salę");
                Console.WriteLine("3. Usuń salę");
                Console.WriteLine("4. Wyświetl plan sali\n");
                Console.WriteLine("5. Dodaj seans");
                Console.WriteLine("6. Usuń seans");
                Console.WriteLine("7. Modyfikuj seans");
                Console.WriteLine("8. Wyświetl wszystkie seanse");
                int iOdp = PodajInt("Co chcesz zrobić? ", 1, 8);

                switch (iOdp)
                {
                    case 1: return;
                    case 2: kino.DodajSale(); break;
                    case 3: kino.UsunSale(); break;
                    case 4: kino.WyswietlSale(); break;
                    case 5: kino.DodajSeans(); break;
                    case 6: kino.UsunSeans(); break;
                    case 7: kino.ModyfikujSeans(); break;
                    case 8: kino.WyswietlWszystkieSeansy(); break;
                }

                kino.ZapiszDane();

                Console.WriteLine("\n-- naciśnij dowolny przycisk --");
                Console.ReadKey();
            }
        }

        public static void MenuRezerwacji(Kino kino)
        {
            Console.Clear();
            Console.WriteLine("1. Wróć do wyboru modułu\n");
            Console.WriteLine("2. Zarejestruj się");
            Console.WriteLine("3. Zaloguj się");
            int odp = PodajInt("Co chcesz zrobić? ", 1, 3);

            Konto zalogowaneKonto = new Konto();

            switch (odp)
            {
                // spróbować przekazać do Rejestracja i Logowanie zalogowaneKonto jako referencję
                case 1: return;
                case 2: zalogowaneKonto = kino.Rejestracja(); break;
                case 3: zalogowaneKonto = kino.Logowanie(); break;
            }

            if (!listaUzytkownikow.Contains(zalogowaneKonto)) return;
            else
            {
                Console.WriteLine("\nZALOGOWANO");
                Console.WriteLine($"WITAJ {zalogowaneKonto.imie} {zalogowaneKonto.nazwisko}");
                Console.WriteLine("\n-- naciśnij dowolny przycisk --");
                Console.ReadKey();
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"WITAJ {zalogowaneKonto.imie} {zalogowaneKonto.nazwisko}");
                Console.WriteLine("1. Wróć do wyboru modułu\n");
                Console.WriteLine("2. Wyświetl seanse do wyboru");
                Console.WriteLine("3. Wyszukaj seans po tytule filmu\n");
                Console.WriteLine("4. Anuluj rezerwację");
                odp = PodajInt("Co chcesz zrobić? ", 1, 4);

                switch (odp)
                {
                    case 1: return;
                    case 2: kino.WyswietlSeanseDoWyboru(zalogowaneKonto.email); break;
                    case 3: kino.WyszukajPoTytule(zalogowaneKonto.email); break;
                    case 4: kino.AnulujRezerwacje(zalogowaneKonto.email); break;
                }

                kino.ZapiszDane();

                Console.WriteLine("\n-- naciśnij dowolny przycisk --");
                Console.ReadKey();
            }
        }

        static void Main(string[] args)
        {
            GlowneMenu();
        }
    }
}
