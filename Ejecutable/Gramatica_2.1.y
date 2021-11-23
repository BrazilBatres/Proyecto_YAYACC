S : L '=' R
  | R
  |	
  ;
L : '*' R
  | 'i' 'd'
  ;
R : L
  ;
S :
  |R
  |'\t';