lines	: lines expr '\n' 
	| lines '\n'
	| 'e'
	| '\e'
	| 'a'
	| 'e'
	;
lines : 'e'
      | hola;
lines : 'h'
      | 'h'
	  | eppa;
expr	: expr '+' expr	
	| expr '*' expr 
	| '(' expr ')'  
	| 'N''U''M';