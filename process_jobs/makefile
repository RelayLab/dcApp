SRC=$(shell find . -name '*.vb')	#имена всех файлов vb в папке и подпапках
CFLAGS=-out:make_jobs.exe
LDFLAGS=-r:dcUser.dll -libpath:../dcUser/
#
all: $(SRC)
	vbnc $(SRC) $(CFLAGS) $(LDFLAGS)
	
clean: 
	rm make_jobs.exe
