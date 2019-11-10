domains
	list = symbol*
predicates
	nondeterm conc(list, list, list).
	nondeterm member(symbol, list, list, list).
	add_to_head(symbol, list, list).
	nondeterm add_to_tail(list, symbol, list).
	
	nondeterm included(symbol, list).
	nondeterm first_func(symbol, symbol, list, list).
	
	nondeterm last_elem(symbol, list).
	nondeterm second_func(symbol, symbol, list, list).
	
	nondeterm third_func(symbol, list, list).
clauses
	conc([], List, List).
	conc([Head|Tail], List, [Head|Rest]) :- conc(Tail, List, Rest).
	
	included(X, [X|Tail]).
	included(X, [Head| Tail]) :- included(X, Tail).
	
	member(X, L, L1, L2) :- conc(L1, [X|L2], L).
	
	add_to_head(X, L, [X|L]).
	
	add_to_tail([], X, [X]).
	add_to_tail([L1|T1], Elem, [L1|T2]) :- add_to_tail(T1, Elem, T2).
	
	last_elem(X, [X]).
	last_elem(X, [Y|Tail]) :- last_elem(X, Tail).

	first_func(X, Y, List, List) :- not(included(Y, List)).
	first_func(X, Y, List, Result) :- member(Y, List, Pre, Post), first_func(X, Y, Post, Post2), add_to_tail(Pre, X, L1), add_to_head(Y, Post2, L2), conc(L1, L2, Result).
	
	second_func(Y1, Y2, List, Result) :- last_elem(X, List), member(X, List, Pre, Post), add_to_tail(Pre, Y1, Pre2), add_to_tail(Pre2, Y2, Pre3), add_to_tail(Pre3, X, Result). 
	
	third_func(X, [Y|Tail], Result) :- X <= Y, add_to_head(X, [Y|Tail], Result).
	third_func(X, [Y|Tail],Result) :- X > Y, third_func(X, Tail, List), add_to_head(Y, List, Result).
	third_func(X, [], Result) :- add_to_head(X, [], Result).
	
goal
	third_func(m, [l,m,n,o,p,q], Result).

	
	
	
	
	