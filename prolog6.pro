domains
	file = input_file; output_file
predicates
	nondeterm after_symbol(string, char).
	nondeterm before_symbol(string, char).
	
	nondeterm processing(file).
	nondeterm main(string, string).
clauses
	after_symbol("", _).
	after_symbol(String, Symbol) :-
		frontchar(String, Symbol, String1),
		frontchar(String1, X, _),
		write(X),
		after_symbol(String1, Symbol).
	after_symbol(String, Symbol) :-
		frontchar(String, _, String1),
		after_symbol(String1, Symbol).
		
	before_symbol("", _).
	before_symbol(String, Symbol) :-
		frontchar(String, X, String1),
		frontchar(String1, Symbol, _),
		write(X),
		before_symbol(String1, Symbol).
	before_symbol(String, Symbol) :-
		frontchar(String, _, String1),
		not(frontchar(String1, 'a', _)),
		before_symbol(String1, Symbol).
		
		
	processing(File) :-
		not(eof(File)), !,
		readln(Line),
		before_symbol(Line, 'a'),
		nl,
		processing(File).
	processing(_).
	
	
	main(InputFile, OutputFile) :-
		existfile(InputFile), !,
		openread(input_file, InputFile),
		readdevice(input_file),

		openwrite(output_file, OutputFile),
		writedevice(output_file),
		
		processing(input_file),
		closefile(input_file),
		closefile(output_file),
		writedevice(screen),
		write("Program completed successful!\n").
	main(InputFile, _) :-
		write("File named  ", InputFile, "  doesn`t exist!\n"). 
		
 goal
	main("D:\\Prolog\\example.txt", "D:\\Prolog\\result.txt").