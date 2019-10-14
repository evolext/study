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
database
	pupil(name, surname, clas, marks)
predicates
	nondeterm choice(integer).
	nondeterm menu.
	nondeterm repeat.

	nondeterm clasList(symbol, symbol, clas).
	nondeterm whichName(symbol, symbol).
	nondeterm excellentPupil(symbol, symbol).
	nondeterm goodPupil(symbol, symbol).
	nondeterm whichMarks(symbol, symbol, marks).

clauses 	
	whichMarks(Name, Surname, Marks) :-
		pupil(Name, Surname, _, Marks).
	excellentPupil(Name, Surname) :-
		pupil(Name, Surname, _, marks(maths(5), history(5), geography(5), biology(5), physics(5), chemistry(5), social(5), nativeLang(5))).
	goodPupil(Name, Surname) :-
		pupil(Name, Surname, _, marks(maths(A), history(B), geography(C), biology(D), physics(E), chemistry(F), social(G), nativeLang(H))),
		A > 3, B > 3, C > 3, D > 3, E > 3, F > 3, G > 3, H > 3.
	whichName(Name, Surname) :-
		pupil(Name, Surname, _, _).
	clasList(Name, Surname, clas(Number, Letter)) :-
		pupil(Name, Surname,clas(Number, Letter), _).

	% Exit program
	choice('0') :-
		write("Exit. . ."),
		nl.

	% Add pupil
	choice('1') :-
		write("Enter name\n"),
		readln(Name),
		write("Enter surname\n"),
		readln(Surname),
		write("Enter class number\n"),
		readint(Number),
		write("Enter class letter\n"),
		readchar(Letter),
		write("Enter maths mark\n"),
		readint(Math),
		write("Enter history mark\n"),
		readint(History),
		write("Enter geography mark\n"),
		readint(Geography),
		write("Enter biology mark\n"),
		readint(Biology),
		write("Enter physics mark\n"),
		readint(Physics),
		write("Enter chemistry mark\n"),
		readint(Chemistry),
		write("Enter social mark\n"),
		readint(Social),
		write("Enter native lang mark\n"),
		readint(Nativelang),
		assert(pupil(Name, Surname, clas(Number, Letter), marks(maths(Math), history(History), geography(Geography), biology(Biology), physics(Physics), chemistry(Chemistry), social(Social), nativeLang(Nativelang)))).

	% Delete pupil by name and surname
	choice('2') :-
		write("Enter pupil name\n"),
		readln(Name),
		write("Enter pupil surname\n"),
		readln(Surname),
		retract(pupil(Name, Surname, _, _)).
	
	% Print pupil list
	choice('3') :-
		pupil(Name, Surname, clas(Number, Letter), marks(maths(Math), history(History), geography(Geography), biology(Biology), physics(Physics), chemistry(Chemistry), social(Social), nativeLang(Nativelang))),
		write("Pupil ",Name," ",Surname," is studying at ",Number,Letter," class and has the following marks: Math - ",Math,", history - ",History,", Geography - ",Geography,", Biology - ",Biology,", Physics - ",Physics,", Chemistry - ",Chemistry,", Social - ",Social,", Native language - ",Nativelang),
		nl.
	
	% which marks
	choice('4') :-
		write("Enter pupil name\n"),
		readln(Name),
		write("Enter pupil surname\n"),
		readln(Surname),
		whichMarks(Name, Surname, Marks),
		write("Pupil ",Name," ",Surname," has: ", Marks).
		
	% Print excellent pupils
	choice('5') :-
		write("\nExcellent pupil list:\n"),
		excellentPupil(Name, Surname),
		write(Name," ",Surname),
		nl.

	% Print good pupils
	choice('6') :-
		write("\nGood pupil list:\n"),
		goodPupil(Name, Surname),
		write(Name," ",Surname),
		nl.

	% Print class list
	choice('7') :-
		write("\nEnter class number\n"),
		readint(Number),
		write("Enter class letter\n"),
		readchar(Letter),
		write("In ",Number,Letter," class are sudy:\n"),
		clasList(X, Y, clas(Number, Letter)),
		write(X," ",Y),
		nl.

	% Load database
	choice('r') :-
		write("Enter filename\n"),
		readln(File),
		existfile(File), !,
		consult(File),
		write("Database read success\n");
		write("File read error. . .\n").
		
	% Save database
	choice('w') :-
		write("\nEnter filename\n"),
		readln(File),
		save(File),
		write("success").


	% Program menu
	menu :-
		repeat,
		write("__________________________________\n"),
		write("Choose the function\n"),
		write("1 - add pupil to base\n"),
		write("2 - delete pupil from base\n"),
		write("3 - print pupil list\n"),
		write("4 - print pupil marks\n"),
		write("5 - print excellent pupils\n"),
		write("6 - print good pupils\n"),
		write("7 - print class list\n"),
		write("r - load database\n"),
		write("w - write database\n"),
		write("0 - exit program\n"),
		readchar(Choice),
		choice(Choice),
		Choice='0'.
	repeat.
	repeat :- repeat.
goal
	menu.