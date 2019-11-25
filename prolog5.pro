domains
	p=p(symbol,symbol)
	vertex=symbol*
	edges=p*
	graph=graph(vertex, edges)
predicates
	nondeterm member(p, edges).
	nondeterm member_vertex(symbol, vertex).
	nondeterm adjacent(symbol, symbol, graph).
	
	nondeterm path(symbol, symbol, graph, vertex).
	nondeterm pathSuffix(symbol, vertex, graph, vertex).
	
	nondeterm hascycle(graph).
	nondeterm not_connected(graph).
	
	nondeterm tree(graph).
clauses
	member(Edge, [Edge|_]).
	member(Edge, [Head|Tail]) :- member(Edge, Tail).
	
	member_vertex(X, [X|_]).
	member_vertex(X, [Head|Tail]) :- member_vertex(X, Tail).
	
	
	adjacent(X, Y, graph(_, Edges)) :-
		member(p(X,Y), Edges);
		member(p(Y,X), Edges).
	
	path(A, Z, Graph, Path) :-
		pathSuffix(A, [Z], Graph, Path).
		
	pathSuffix(A, [A|Path1], _, [A|Path1]).
	pathSuffix(A, [Y|Path1], Graph, Path) :-
		adjacent(X, Y, Graph),
		not(member_vertex(X, Path1)),
		pathSuffix(A, [X,Y|Path1], Graph, Path).
	
	hascycle(graph(Vertex, Edges)) :-
		adjacent(X,Y, graph(Vertex, Edges)),
		path(X,Y,graph(Vertex, Edges), [X,_,_|_]).
		
	not_connected(graph(Vertex, Edges)) :-
		member_vertex(A, Vertex), member_vertex(B, Vertex), not(path(A,B, graph(Vertex,Edges), _)).
		
	tree(Graph) :-
		not(not_connected(Graph)),
		not(hascycle(Graph)).
	
goal
	
	





	
	