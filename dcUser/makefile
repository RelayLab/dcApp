SRC=$(shell find . -name '*.vb')	#имена всех файлов vb в папке и подпапках
#OBJ=$(SRC:.c=.o)	#заменить .с на .о
#ELF=$(SRC:.c=.elf)
#LST=$(SRC:.c=.lst)
#MMCU=atmega328p
#CFLAGS=-Os -mmcu=$(MMCU) -g
#LDFLAGS=
#
all: $(SRC)
	vbnc -t:library -out:dcUser.dll $(SRC) 
	
clean:
	rm dcUser.dll
