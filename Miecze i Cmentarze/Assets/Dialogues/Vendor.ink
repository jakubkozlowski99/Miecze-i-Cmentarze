-> main
 == main ==
Hej młody wędrowcze, czego potrzebujesz? Mikstury, może jakiś magiczny pierścień? Mam wszystko czego zapragniesz!
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
