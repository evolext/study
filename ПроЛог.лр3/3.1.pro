database
	lang(symbol)
	know(symbol, symbol)
predicates
	nondeterm menu.
	nondeterm choice(integer).
	nondeterm repeat.
clauses
	choice('0') :- 
		save("D:\\base.dba"),
		write("Exit..").
	
	% Add people
	choice('1') :-
		write("Enter name"),
		nl,
		readln(Name),
		nl,
		lang(Lang),
		write("Does he know ", Lang," langiuage?"),
		readln(Answer),
		Answer=y,
		assert(know(Name, Lang)).
	% Add language
	choice('2') :-
		write("Enter name of language"),
		nl,
		readln(Lang),
		assert(lang(Lang)).
	% Print people date
	choice('3') :-
		know(Name, Lang),
		write(Name, " knows ", Lang, " language\n").
	% Print language date
	choice('4') :-
		lang(Lang),
		write(Lang),
		nl.
	% Save database
	choice('w') :-
		write("Enter name of file\n"),
		readln(File),
		save(File),
		write("Success!\n").
	% Read database
	choice('r') :-
		write("Enter name of file\n"),
		readln(File),
		existfile(File), !,
		consult(File),
		write("Database read success\n");
		write("File read error. . .\n").
		
	menu :-
		repeat,
		write("Select an action\n"),
		write("1. Add people\n"),
		write("2. Add language\n"),
		write("3. Print people date\n"),
		write("4. Print language date\n"),
		write("w - save database\n"),
		write("r - read database\n"),
		write("0. Exit\n"),
		readchar(Choice),
		choice(Choice),
		Choice='0'.
	repeat.
	repeat :- repeat.
	
goal
	menu.
		
	