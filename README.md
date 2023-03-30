# AlgorytmGenetycznyWPF
Aplikacja okienkowa C# WPF stworzona jako zalicznie przedmiotu na studiach będącego wprowadzeniem do sztucznej inteligencji

Program rozwiązuje problem znalezienia w przedziale <-4; 12> argument, dla którego funkcja
F(x)= x MOD 1 * (COS( 20 * π * x) – SIN(x)) przyjmuje największą wartość.

Prawidłowe rozwiązanie tego problemu to F(x) = 10.999..., x = 1.999...

Kolejne kroki działania:
1. Kodowanie osobnika,
2. Losowa inicjalizacja populacji początkowej,
3. ocena osobników w populacji posługując się funkcją oceny oraz funkcją dopasowania,
4. selekcja osobników (wybór jedynie osobników najbliższych największej wartości w zadanym przedziale, bazując na ich ocenie),
5. wybór osobników oraz ich krzyżowanie na podstawie parametru PK,
6. Mutacja osobników na podstawie parametru PM,
7. Tworzenie podsumowania wyników działania algorytmu genetycznego (wypisanie minimalnej, średniej i maksymalnej wartości funkcji oceny w każdym pokoleniu oraz stworzenie na ich podstawie wykresu)


parametry przyjmowane przez program na wejściu:
 1. a,b - przedział przeszukiwanych argumentów,
 2. d - dokładność, ilość liczb po przecinku wypisywanych w wynikach końcowych,
 3. N - rozmiar populacji, ilość osobników w pierwszym pokoleniu,
 4. PK - prawdopodobieństwo krzyżowania - procentowa szansa na wybranie osobnika na rodzica
 5. PM - prawdopodobieństwo mutacji - procentowa szansa na mutację każdego pojedynczego bitu osobnika, dzięki której bit zmienia wartość na przeciwną
 6. T - ilość pokoleń, każde z nich będzie dążyło do wyniku jak najbardziej zbliżonego do oczekiwanego
 7. elita - najlepszy osobnik z danego pokolenia ma gwarantowane miejsce w pokoleniu nastepnym bazując na najlepszym wyniku funkcji oceny
 
Dla dużych populacji (N), które mają dużo pokoleń (T) na dotarcie do najlepszego wyniku rezultaty są najlepsze, jest największa szansa na znalezienie prawidłowego wyniku.
Przykładowa poprawna kombinacja to N=80, T=150, PK = 0,85, PM = 0,0025.
Wynik należy interpretować jako skumulowane rozwiązania do których dotarły osobniki, w ostatniej kolumnie przedstawiona jest procentowa część spośród wszystkich rozwiązań.

W zakładce wykres przedstawione są wartości każdego pokolenia po kolei: na czerwono przedstawiona jest maksymalna wartość, na zielono wartość średnia, a na niebiesko minimalna, w obrębie jednego pokolenia.
