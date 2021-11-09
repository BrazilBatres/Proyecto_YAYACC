Pasos para la ejecución:
1. Abrir el CMD en esta carpeta.
2. Ya en el CMD -> Escribir: YAYACC <archivo.y>
	- (El <archivo.y> debe de estar en esta carpeta).
3. Presionar ENTER.
4. Se desplegará uno de estos 3 mensajes:
	- Lex Error: En caso existiera un error léxico.
	- Syntax Error: En caso existiera un error sintáctico.
	- Expresión OK: En caso la gramática ingresada desde el archivo.y es correcta (se imprimirá la gramática, mostrando sus variables y sus respectivas reglas).
--------------------------------------------------------------------------------------------------------------------------------------
NOTA: En el paso No. 4, en caso la gramática ingresada esté OK... Puede que a veces no imprima bien la gramática en la consola
porque esta etapa aún está en desarrollo. Por favor solamente considerar la parte de la validación de la gramática, no la conversión
a objeto gramática como tal.
--------------------------------------------------------------------------------------------------------------------------------------