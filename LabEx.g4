grammar LabEx;

/*
 * Parser Rules
 */

compileUnit : expression EOF;

expression :
	LPAREN expression RPAREN #ParenthesizedExpr
	| expression EXPONENT expression #ExponentialExpr
    | expression operatorToken=(MULTIPLY | DIVIDE) expression #MultiplicativeExpr
	| expression operatorToken=(ADD | SUBTRACT) expression #AdditiveExpr
	| operatorToken=(NMAX | NMIN) LPAREN expression (DOT expression)* RPAREN #NmaxNminExpr
	| operatorToken=(INC | DEC) LPAREN expression RPAREN #IncDecExpr
	| NUMBER #NumberExpr
	//| IDENTIFIER #IdentifierExpr
	; 

/*
 * Lexer Rules
 */

NUMBER : INT ('.' INT)?; 
//IDENTIFIER : [a-zA-Z]+[1-9][0-9]+;

INT : ('0'..'9')+;

EXPONENT : '^';
MULTIPLY : '*';
DIVIDE : '/';
SUBTRACT : '-';
ADD : '+';
LPAREN : '(';
RPAREN : ')';
DOT : ',';
NMAX : 'nmax';
NMIN : 'nmin';
INC : 'inc';
DEC : 'dec';

WS : [ \t\r\n] -> channel(HIDDEN);
