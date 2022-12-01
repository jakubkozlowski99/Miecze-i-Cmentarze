-> main
 == main ==
Witaj, czego potrzebujesz wędrowcze?
    + [Pokaż mi swoje towary.] #action:shop
        -> main
    + [Masz dla mnie jakieś zadanie?]
        -> chosen("Bulbasaur")
    + [Bywaj.]
        -> END
        
== chosen(pokemon) ==
You chose {pokemon}!
    + [Wróć.]
        -> main
    + [Żegnaj.]
        -> END
-> END
