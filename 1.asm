.386
.MODEL FLAT, STDCALL

EXTERN GetStdHandle@4 : PROC
EXTERN ReadConsoleA@20 : PROC
EXTERN ExitProcess@4: PROC
EXTERN WriteConsoleA@20: PROC
EXTERN CharToOemA@8: PROC
EXTERN lstrlenA@4: PROC

.DATA
STR1 DB "Введите первое число: ", 13,10,0
STR2 DB "Введите второе число: ", 13,10,0
LENS DD ?   ; кол-во введенных символов
BUF DB 255 dup(?)
SIGN DD ?   ; переменная для хранения знака
STRN DB "-", 255 dup(?) ; строка для вывода
DIN DD ?    ; переменная для дескрпитора ввода
DOUT DD ?   ; переменная для дескриптора вывода
TEMP DD ?   ; переменная для временного хранения первого операнда и ответа

.CODE
MAIN PROC

; Перекодируем первую выводимую строку
MOV EAX, OFFSET STR1
PUSH EAX
PUSH EAX
CALL CharToOemA@8
; Перекодируем вторую выводимую строку
MOV EAX, OFFSET STR2
PUSH EAX
PUSH EAX
CALL CharToOemA@8

; Получим дескриптор вывода
PUSH -11
CALL GetStdHandle@4
MOV DOUT, EAX

; Получаем дескриптор ввода
PUSH -10;
CALL GetStdHandle@4
MOV DIN, EAX

; вывод первой строки
PUSH OFFSET STR1    ; в стек помещается адрес строки
CALL lstrlenA@4     ; длина первой строки в EAX
; сам вывод
PUSH 0
PUSH OFFSET LENS
PUSH EAX
PUSH OFFSET STR1
PUSH DOUT
CALL WriteConsoleA@20


; Ввод первого числа
PUSH 0
PUSH OFFSET LENS
PUSH 255
PUSH OFFSET BUF
PUSH DIN
CALL ReadConsoleA@20


CALL CONVERTING   ; преобразование строки в число
MOV TEMP, EAX     ; сохраняем первый операнд


; вывод второй строки
PUSH OFFSET STR2    ; в стек помещается адрес строки
CALL lstrlenA@4     ; длина первой строки в EAX
; сам вывод
PUSH 0
PUSH OFFSET LENS
PUSH EAX
PUSH OFFSET STR2
PUSH DOUT
CALL WriteConsoleA@20

; Ввод второго числа
PUSH 0
PUSH OFFSET LENS
PUSH 255
PUSH OFFSET BUF
PUSH DIN
CALL ReadConsoleA@20

CALL CONVERTING     ; преобразование второй строки в число
SUB TEMP, EAX       ; вычитание двух чисел




; вывод ответа
MOV ESI, OFFSET STRN ; Указатель на строку вывода
XOR ECX, ECX	     ; В ECX будем хранить длину получившегося числа

CMP TEMP, 0 ; если ответ отрицательный
JGE output
; перевод из дполнительного кода в прямой
SUB TEMP, 1
XOR TEMP, -1
INC ESI 
INC ECX     ; в строке будем хранить знак "минус"



output:
MOV EAX, TEMP 	; Результат вычитания поместим в регистр


; перевод разности в 10СС путем нахождения остатка от деления
hex_to_dec:
	CMP EAX, 10
	JL next         ; если число < 10, то выходим из цикла

	MOV EDX, 0 		;Старшая часть делимого
	MOV EBX, 10 	;Делитель
	DIV EBX         ;Частное в EAX
			        ;Остаток в EDX
    ADD EDX, '0'
	PUSH EDX
	INC ECX
	JMP hex_to_dec



next:
	INC ECX         ; перевод последней цифры числа
    ADD EAX, '0'
	PUSH EAX ; в этот момент готовое число в десятичной СС хранится в стеке в обратном порядке
			 ; ECX = количество разрядов полученного числа


   
 
MOV LENS, ECX ; длина выводимой строки, оно же счетчик цикла
; выталкиваем число из стека в строку ответа
add_to_str:
    pop [ESI]   ; добавляем из стека в строку
    INC ESI
    LOOP add_to_str

; вывод строки ответа
PUSH 0
PUSH OFFSET LENS
PUSH LENS
PUSH OFFSET STRN
PUSH DOUT
CALL WriteConsoleA@20

; выход из программы
PUSH 0
CALL ExitProcess@4
MAIN ENDP

;---------Процедура конвертации строки в число-----------------------------------------------------------

CONVERTING PROC
    MOV SIGN, 1     ; изначально считаем, что число положительное

    MOV EDI, 16         ; система счисления
    MOV ECX, LENS       ; счетчик цикла = длине строки
    MOV ESI, OFFSET BUF	; помещаем в ESI указатель на начало введенной строки

    XOR EBX, EBX	; обнуляем BX
    XOR EAX, EAX	; обнуляем AX

    ; проверка на минус
    MOV BL, [ESI]
    CMP BL, 45
    JNZ cycle_body ; если положительное число
    MOV SIGN, -1   ; меняем знак числа на "минус"
    INC ESI        ; длина строки из-за этого увеличилась

    ; цикл перевода
    CONVERT:             
	    MOV BL, [ESI]	; помещаем символ из строки в BL
 
    cycle_body:         ; если символ - цифра
	    CMP BL, 48 
	    JL cycle_exit   ; BL < 48 => выходим из цикла, так как символ находится вне диапазона
	    CMP BL, 57
	    JG is_uppercase_letter    ; BL > 57 => это не цифра, а буква
	    ; символ строки - цифра
	    SUB BL, '0'
	    MUL DI
	    ADD EAX, EBX
	    JMP next

	    is_uppercase_letter:    ; если символ - прописная буква
		    CMP BL, 65 
		    JL cycle_exit   ; BL < 65 => в диапазоне [57-65] , т.е. не буква
		    CMP BL, 70
		    JG is_lowercase_letter   ; BL > 70 => это, возможно, строчная буква
		    ; символ строки - прописная буква
		    SUB BL, 'A'
		    MUL EDI
		    ADD EAX, EBX
		    ADD EAX, 10
            JMP next

        is_lowercase_letter:    ; если символ - строчная буква
            CMP BL, 97
            JL cycle_exit   ; BL < 97 => в диапазоне [65,97], т.е. не буква
            CMP BL, 102
            JG cycle_exit   ; не буква
            ; символ строки - строчная буква
            SUB BL, 'a'
            MUL EDI
            ADD EAX, EBX
            ADD EAX, 10
    
	    next:
		    INC ESI      ; переход к следующему символу
		    LOOP CONVERT ; возврат к началу цикла

        cycle_exit:
            IMUL SIGN     ; устанавливаем знак
            RET           ; выход
CONVERTING ENDP

;-------------------------------------------------------------------------------------------------------------

END MAIN