INCLUDE globals.ink

{ questState == 0 : -> main | questState == 1 : -> main2}

 == main ==
Witaj, czego potrzebujesz wędrowcze?
    + [Pokaż mi swoje towary.] #action:shop
        -> main
    + [Masz dla mnie jakieś zadanie?] #action:quest_ask
        -> quest
    + [Bywaj.]
        -> END
        
== quest ==
    + [Zrobię to.] #action:quest_give
        -> END
    + [Może innym razem.]
        -> END
-> END

== main2 ==

Witaj, czego potrzebujesz wędrowcze?
    + [Pokaż mi swoje towary.] #action:shop
        -> main
    + [Jeśli chodzi o to zadanie..] #action:quest_ask
        -> quest_notfinished
    + [Bywaj.]
        -> END
-> END

== quest_notfinished ==

Daj znać jak je wykonasz.
    + [Oczywiście, zabieram się do pracy.]
        -> END
    + [Wróć.]
        -> main2
-> END
