domains
	name = symbol.
	surname = symbol.
	clas = clas(integer, char).
	
	maths = maths(integer).
	history = history(integer).
	geography = geography(integer).
	biology = biology(integer).
	physics = physics(integer).
	chemistry = chemistry(integer).
	social = social(integer).
	nativeLang = nativeLang(integer).
	
	marks = marks(maths, history, geography, biology, physics, chemistry, social, nativeLang).
predicates
	nondeterm pupil(name, surname, clas, marks).
	
	nondeterm clasList(symbol, symbol, clas).
	nondeterm whichClas(symbol, symbol, clas).
	nondeterm whichName(symbol, symbol).
	nondeterm namesakesList(symbol).
	nondeterm excellentPupil(symbol, symbol).
	nondeterm goodPupil(symbol, symbol).
	nondeterm whichMarks(symbol, symbol, marks).
	nondeterm inOneClas(symbol, symbol, symbol, symbol).

clauses 
	inOneClas(NameFirst, SurnameFirst, NameSecond, SurnameSecond) :-
		pupil(NameFirst, SurnameFirst, clas(Number, Letter), _),
		pupil(NameSecond, SurnameSecond, clas(Number, Letter), _),
		NameFirst <> NameSecond.
		
	whichMarks(Name, Surname, Marks) :-
		pupil(Name, Surname, _, Marks).
	excellentPupil(Name, Surname) :-
		pupil(Name, Surname, _, marks(maths(5), history(5), geography(5), biology(5), physics(5), chemistry(5), social(5), nativeLang(5))).
	goodPupil(Name, Surname) :-
		pupil(Name, Surname, _, marks(maths(A), history(B), geography(C), biology(D), physics(E), chemistry(F), social(G), nativeLang(H))),
		A > 3, B > 3, C > 3, D > 3, E > 3, F > 3, G > 3, H > 3.
	whichName(Name, Surname) :-
		pupil(Name, Surname, _, _).
	namesakesList(Surname):-
		pupil(NameX, Surname, _, _),
		pupil(NameY, Surname, _, _),
		NameX <> NameY.

	whichClas(Name, Surname, Clas) :-
		pupil(Name, Surname, Clas, _).

	clasList(Name, Surname, clas(Number, Letter)) :-
		pupil(Name, Surname,clas(Number, Letter), _).


	/* 11 A, eighteen students */
	pupil("Modest", "Kuroptev", clas(11, 'A'), marks(maths(5), history(4), geography(4), biology(4), physics(4), chemistry(4), social(5), nativeLang(3))).
	pupil("Agafon", "Rubanov", clas(11, 'A'), marks(maths(4), history(4), geography(3), biology(3), physics(5), chemistry(4), social(3), nativeLang(3))).
	pupil("Oleg", "Habibullin", clas(11, 'A'), marks(maths(4), history(4), geography(4), biology(4), physics(5), chemistry(5), social(4), nativeLang(3))).
	pupil("Aristarh", "Koshkin", clas(11, 'A'), marks(maths(4), history(3), geography(3), biology(3), physics(4), chemistry(5), social(4), nativeLang(5))).
	pupil("Arsenij", "Kozhevnikov", clas(11, 'A'), marks(maths(4), history(3), geography(3), biology(5), physics(3), chemistry(3), social(3), nativeLang(4))).
	pupil("Semen", "Zujkov", clas(11, 'A'), marks(maths(3), history(3), geography(3), biology(3), physics(3), chemistry(3), social(3), nativeLang(4))).
	pupil("Nikolaj", "Ustyuzhanin", clas(11, 'A'), marks(maths(4), history(5), geography(5), biology(5), physics(5), chemistry(5), social(4), nativeLang(5))).
	pupil("Valeryan", "Ushakov", clas(11, 'A'), marks(maths(5), history(4), geography(5), biology(5), physics(3), chemistry(4), social(5), nativeLang(5))).
	pupil("Adrian", "Merzlov", clas(11, 'A'), marks(maths(5), history(4), geography(4), biology(5), physics(5), chemistry(5), social(4), nativeLang(4))).
	pupil("Yurij", "Kolobov", clas(11, 'A'), marks(maths(3), history(4), geography(4), biology(3), physics(4), chemistry(3), social(3), nativeLang(4))).
	pupil("Lyubov", "Mosencova", clas(11, 'A'), marks(maths(5), history(5), geography(4), biology(5), physics(4), chemistry(5), social(3), nativeLang(3))).
	pupil("Tamara", "Oleneva", clas(11, 'A'), marks(maths(5), history(5), geography(5), biology(5), physics(5), chemistry(5), social(5), nativeLang(5))).
	pupil("Vladislava", "Belomestnaya", clas(11, 'A'), marks(maths(3), history(5), geography(3), biology(5), physics(5), chemistry(4), social(3), nativeLang(3))).
	pupil("Inna", "Bogrova", clas(11, 'A'), marks(maths(3), history(4), geography(5), biology(5), physics(4), chemistry(5), social(5), nativeLang(3))).
	pupil("Anna", "Dvurechenskaya", clas(11, 'A'), marks(maths(4), history(4), geography(3), biology(4), physics(5), chemistry(4), social(5), nativeLang(5))).
	pupil("Anastasiya", "Obolenskaya", clas(11, 'A'), marks(maths(4), history(3), geography(5), biology(5), physics(5), chemistry(4), social(4), nativeLang(5))).
	pupil("Nataliya", "Yakubenko", clas(11, 'A'), marks(maths(5), history(5), geography(4), biology(3), physics(5), chemistry(4), social(3), nativeLang(3))).
	pupil("Inga", "Zhvanec", clas(11, 'A'), marks(maths(5), history(5), geography(4), biology(4), physics(4), chemistry(4), social(4), nativeLang(5))).
	
	/* 10 A, twenty students*/
	pupil("Dina", "Molodcova", clas(10, 'A'), marks(maths(5), history(3), geography(4), biology(4), physics(3), chemistry(5), social(4), nativeLang(5))).
	pupil("Lyubov", "Reshetilova", clas(10, 'A'), marks(maths(5), history(5), geography(4), biology(4), physics(4), chemistry(4), social(5), nativeLang(5))).
	pupil("Margarita", "Doncova", clas(10, 'A'), marks(maths(5), history(5), geography(4), biology(3), physics(4), chemistry(5), social(3), nativeLang(3))).
	pupil("Yana", "Pustohodova", clas(10, 'A'), marks(maths(4), history(4), geography(4), biology(4), physics(4), chemistry(4), social(4), nativeLang(5))).
	pupil("Yaroslava", "Krupnova", clas(10, 'A'), marks(maths(4), history(4), geography(4), biology(4), physics(3), chemistry(3), social(5), nativeLang(4))).
	pupil("Vladislav", "Ponomaryov", clas(10, 'A'), marks(maths(3), history(4), geography(4), biology(5), physics(3), chemistry(5), social(4), nativeLang(3))).
	pupil("Hariton", "Kazarezov", clas(10, 'A'), marks(maths(4), history(5), geography(5), biology(5), physics(3), chemistry(4), social(5), nativeLang(4))).
	pupil("Lukyan", "Chikunov", clas(10, 'A'), marks(maths(4), history(5), geography(4), biology(5), physics(5), chemistry(4), social(4), nativeLang(5))).
	pupil("Semen", "Balandin", clas(10, 'A'), marks(maths(4), history(5), geography(3), biology(4), physics(5), chemistry(3), social(5), nativeLang(5))).
	pupil("Artem", "Yagafarov", clas(10, 'A'), marks(maths(3), history(5), geography(3), biology(4), physics(5), chemistry(3), social(4), nativeLang(5))).
	pupil("Agafon", "Pogrebnov", clas(10, 'A'), marks(maths(5), history(5), geography(4), biology(4), physics(4), chemistry(5), social(5), nativeLang(4))).
	pupil("Kondrat", "Muhametov", clas(10, 'A'), marks(maths(3), history(3), geography(4), biology(4), physics(5), chemistry(3), social(4), nativeLang(4))).
	pupil("Oleg", "Suslov", clas(10, 'A'), marks(maths(5), history(5), geography(5), biology(5), physics(5), chemistry(5), social(5), nativeLang(5))).
	pupil("Nestor", "Dyabin", clas(10, 'A'), marks(maths(4), history(5), geography(4), biology(5), physics(5), chemistry(3), social(5), nativeLang(3))).
	pupil("Stepan", "Shepelev", clas(10, 'A'), marks(maths(4), history(3), geography(4), biology(5), physics(5), chemistry(5), social(5), nativeLang(4))).
	pupil("Igor", "Osipov", clas(10, 'A'), marks(maths(5), history(5), geography(5), biology(5), physics(5), chemistry(5), social(5), nativeLang(5))).
	pupil("Filip", "Skorobogatov", clas(10, 'A'), marks(maths(3), history(5), geography(5), biology(5), physics(3), chemistry(5), social(4), nativeLang(4))).
	pupil("Artem", "Yaromeev", clas(10, 'A'), marks(maths(4), history(3), geography(4), biology(4), physics(4), chemistry(3), social(4), nativeLang(4))).
	pupil("Fedor", "Yakutkin", clas(10, 'A'), marks(maths(3), history(3), geography(4), biology(4), physics(3), chemistry(5), social(4), nativeLang(3))).
	pupil("Sidor", "Lebedev", clas(10, 'A'), marks(maths(3), history(4), geography(5), biology(4), physics(4), chemistry(5), social(5), nativeLang(4))).
	
	/* 10 B, eighteen students */
	pupil("Feliks", "Samojlov", clas(10, 'B'), marks(maths(3), history(3), geography(5), biology(3), physics(4), chemistry(3), social(4), nativeLang(4))).
	pupil("Martyn", "Osipov", clas(10, 'B'), marks(maths(4), history(3), geography(3), biology(3), physics(3), chemistry(4), social(5), nativeLang(3))).
	pupil("Iosif", "Hmelnov", clas(10, 'B'), marks(maths(5), history(3), geography(5), biology(5), physics(3), chemistry(4), social(3), nativeLang(5))).
	pupil("Ernst", "Oborin", clas(10, 'B'), marks(maths(5), history(4), geography(3), biology(3), physics(4), chemistry(5), social(4), nativeLang(5))).
	pupil("Lavr", "Yazykov", clas(10, 'B'), marks(maths(3), history(5), geography(3), biology(3), physics(4), chemistry(4), social(5), nativeLang(3))).
	pupil("Yury", "Kuziev", clas(10, 'B'), marks(maths(5), history(5), geography(4), biology(5), physics(4), chemistry(4), social(5), nativeLang(5))).
	pupil("Mefodij", "Gorbachev", clas(10, 'B'), marks(maths(5), history(5), geography(5), biology(5), physics(5), chemistry(5), social(5), nativeLang(5))).
	pupil("Timur", "Yahontov", clas(10, 'B'), marks(maths(3), history(3), geography(5), biology(5), physics(3), chemistry(5), social(4), nativeLang(4))).
	pupil("Filip", "Ozerov", clas(10, 'B'), marks(maths(4), history(4), geography(4), biology(3), physics(3), chemistry(4), social(5), nativeLang(5))).
	pupil("Vladlen", "Rubcov", clas(10, 'B'), marks(maths(4), history(5), geography(4), biology(3), physics(4), chemistry(3), social(4), nativeLang(4))).
	pupil("Nikon", "Yanson", clas(10, 'B'), marks(maths(5), history(3), geography(3), biology(5), physics(4), chemistry(5), social(4), nativeLang(3))).
	pupil("Samson", "Avdoshkin", clas(10, 'B'), marks(maths(3), history(4), geography(4), biology(4), physics(4), chemistry(5), social(5), nativeLang(3))).
	pupil("Aleksandra", "Yandieva", clas(10, 'B'), marks(maths(5), history(4), geography(4), biology(4), physics(5), chemistry(5), social(4), nativeLang(5))).
	pupil("Tatyana", "Daryushina", clas(10, 'B'), marks(maths(3), history(4), geography(3), biology(3), physics(3), chemistry(5), social(5), nativeLang(5))).
	pupil("Diana", "Kurtashkina", clas(10, 'B'), marks(maths(3), history(5), geography(4), biology(4), physics(3), chemistry(5), social(4), nativeLang(3))).
	pupil("Svetlana", "Verica", clas(10, 'B'), marks(maths(5), history(5), geography(5), biology(5), physics(5), chemistry(5), social(5), nativeLang(5))).
	pupil("Majya", "Azarova", clas(10, 'B'), marks(maths(4), history(4), geography(5), biology(3), physics(5), chemistry(5), social(3), nativeLang(5))).
	pupil("Zlata", "Nikerova", clas(10, 'B'), marks(maths(5), history(5), geography(3), biology(5), physics(4), chemistry(5), social(3), nativeLang(4))).
goal
	clasList(X, Y, clas(11, 'A')).
	
	
	
	
	
	
	
	