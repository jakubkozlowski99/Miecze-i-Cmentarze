INCLUDE globals.ink

{ questState < 2 : -> main |-> main2}

 == main ==
Witaj, czego potrzebujesz wędrowcze?
    + [Pokaż mi swoje towary.] #action:shop
        -> main
    + [Masz dla mnie jakieś zadanie?] #action:quest_ask
        -> quest_available
    + [Bywaj.]
        -> END

== main2 ==

Witaj, czego potrzebujesz wędrowcze?
    + [Pokaż mi swoje towary.] #action:shop
        -> main2
    + [Jeśli chodzi o to zadanie..] #action:quest_ask
        -> quest_started
    + [Bywaj.]
        -> END
-> END

== quest_started ==

{
    - questState == 2:
        Daj znać jak je wykonasz.
        + [Oczywiście, zabieram się do pracy.]
            -> END
        + [Wróć.]
            -> main2
    - else:
        Świetnie! Oto twoja nagroda.
        +[Dziękuję, przyda się.] #action:quest_reward
            -> END
}

== quest_available ==

{
    - questState == 0:
        + [Zrobię to.] #action:quest_give
            -> END
        + [Może innym razem.]
            -> END
    -else:
        Niestety nie mam dla ciebie nic do zrobienia.
        + [Wróć.]
            ->END
}