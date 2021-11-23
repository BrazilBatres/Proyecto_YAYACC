lines : lines expr '\n' 
      | lines '\n'
      | 
      ;		
expr : expr '+' expr	
     | expr '*' expr 
     | '(' expr ')'  
     | 'N' 'U' 'M'
     ;