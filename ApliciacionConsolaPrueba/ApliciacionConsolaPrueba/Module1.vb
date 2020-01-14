
Imports Npgsql

    Module Module1


        Dim MyConection As New Npgsql.NpgsqlConnection()
        Dim numero, numUno, numDos As String
        Dim ope As Integer


        Sub Main()

        menu()


    End Sub

        Public Sub menu()

        Do

            Console.Write("Por favor elija seleccione la operacion a realizar:")
            Console.WriteLine("")
            Console.WriteLine("1.- Suma")
            Console.WriteLine("2.- Recta")
            Console.WriteLine("3.- Multiplicición")
            Console.WriteLine("4.- División")
            Console.WriteLine("5.- Funcion")
            Console.WriteLine("6.- Salir del programa")

            Select Case validarNumero()

                Case 1
                    introducirNumeros()
                    ope = CInt(numUno + numDos)
                    menuOperaciones()

                Case 2
                    introducirNumeros()
                    ope = CInt(numUno - numDos)
                    menuOperaciones()

                Case 3
                    introducirNumeros()
                    ope = CInt(numDos / numUno)
                    menuOperaciones()

                Case 4
                    introducirNumeros()
                    ope = CInt(numUno * numDos)
                    menuOperaciones()

                Case 5
                    introducirNumeros()
                    menuOperaciones()
                Case 6
                    Console.Write("Fin del programa")
            End Select


        Loop While numero < 6


    End Sub

        Public Sub menuOperaciones()

            Console.Write("Por favor indique que desea hacer:")
            Console.WriteLine("")
            Console.WriteLine("1.- Add")
            Console.WriteLine("2.- Buscar")
            Console.WriteLine("3.- Modificar")
            Console.WriteLine("4.- Borrar")
            Console.WriteLine("5.- Salir del programa")
            'numero = Console.ReadLine()

            Select Case validarNumero()

                Case 1
                    add(ope)
                Case 2
                    buscar(ope)
                Case 3
                    delete(ope)
                Case 4
                    Console.WriteLine("Por favor introduzca el numero a modificar")
                    Dim numeroNuevo As Integer = CInt(Console.ReadLine())
                    update(ope, numeroNuevo)
                Case 5
                    Console.Write("Fin del programa")

            End Select


        End Sub

        Public Function validarNumero() As Integer

            Dim bandera As Boolean
            Do
                numero = Console.ReadLine()
                If (String.IsNullOrEmpty(numero)) Then
                    Console.WriteLine("Error, introduzca nuevo el numero del menu")
                    bandera = False
                ElseIf (CInt(numero) < 0 Or CInt(numero) > 6) Then
                    Console.WriteLine("Error, introduzca nuevo el numero del menu")
                    bandera = False
                Else
                    bandera = True
                End If

            Loop While bandera = False

            Return CInt(numero)

        End Function

        Public Sub introducirNumeros()
            Dim bandera As Boolean
            Do
                Console.WriteLine("Escriba por favor el primer numero.")
                numUno = Console.ReadLine()
                Console.WriteLine("Escriba por favor el segundo numero.")
                numDos = Console.ReadLine()

                If String.IsNullOrEmpty(numUno) Or String.IsNullOrEmpty(numDos) Then
                    Console.WriteLine("Revise los numeros los ha introducido mal.")
                    bandera = False
                ElseIf (CInt(numUno) < 0 And CInt(numUno) > 5) Or (CInt(numDos) < 0 And CInt(numDos) > 5) Then

                    bandera = False
                Else
                    bandera = True
                End If

            Loop While bandera = False

        End Sub

        Public Sub conectar()
            Try
                MyConection.ConnectionString = "Server= 127.0.0.1 ; Port= 5433; Database= prueba; user id=postgres; password=Alicia25!;"
                MyConection.Open()
                If MyConection.State = ConnectionState.Open Then
                    Console.WriteLine("Conectado")

                Else
                    Console.WriteLine("Error de conexion")
                End If
            Catch ex As Exception
                Console.WriteLine(Err.Description)
            End Try


        End Sub

        Public Sub add(ope As String)

            If buscar(ope) = False Then
                conectar()
                Dim Sql As String
                Sql = "Insert Into operacion (suma) values(" + ope + ")"
                Dim cmd As New NpgsqlCommand(Sql, MyConection)
                Dim x = cmd.ExecuteNonQuery()
                If x > 0 Then
                    Console.WriteLine("Se ha añadido correctamente")
                Else
                    Console.WriteLine("No se ha añadido correctamente")
                End If

                MyConection.Close()
            End If

        End Sub

        Public Sub delete(ope As String)

            If buscar(ope) Then
                conectar()
                Dim Sql As String
                Sql = "Delete From operacion Where suma = " + ope + ";"
                Dim cmd As New NpgsqlCommand(Sql, MyConection)
                Dim x = cmd.ExecuteNonQuery()
                If x > 0 Then
                    Console.WriteLine("Se ha borrado correctamente")
                Else
                    Console.WriteLine("No se ha borrado correctamente")
                End If

                MyConection.Close()
            End If

        End Sub

        Public Sub update(ope As String, opeNueva As String)

            If buscar(ope) Then
                conectar()
                Dim Sql As String
                Sql = "Update operacion set suma=" + opeNueva + " Where suma = " + ope + ";"
                Dim cmd As New NpgsqlCommand(Sql, MyConection)
                Dim x = cmd.ExecuteNonQuery()
                If x > 0 Then
                    Console.WriteLine("Se ha modificado correctamente")
                Else
                    Console.WriteLine("No se ha modificado correctamente")
                End If

                MyConection.Close()
            End If

        End Sub


        Public Function buscar(ope As String) As Boolean
            conectar()
            Dim Sql As String = "Select count(*) From operacion where suma = " + ope + ";"
            Dim cmd As New NpgsqlCommand(Sql, MyConection)
            Dim reader As NpgsqlDataReader
            reader = cmd.ExecuteReader()
            Dim nuemroEncontrado As Integer

            While reader.Read()
                nuemroEncontrado = reader.Item(0)
            End While
            MyConection.Close()
            If nuemroEncontrado > 0 Then
                Return True
            End If

            Return False
        End Function

        Public Sub mostarTodos()
            conectar()
            Dim Sql As String = "Select suma From operacion"
            Dim cmd As New NpgsqlCommand(Sql, MyConection)
            Dim reader As NpgsqlDataReader
            reader = cmd.ExecuteReader()

            Dim lista As New List(Of String)

            While reader.Read()
                lista.Add(reader.Item(0))
            End While
            MyConection.Close()

            For Each numero As String In lista
                Console.WriteLine(numero.ToString())
            Next

        End Sub

        Public Sub llamarFuncion(ope As String, opeDos As String)
            conectar()
            Dim Sql As String = "Select sumar_numeros('" + ope + "','" + opeDos + "') From operacion"
            Dim cmd As New NpgsqlCommand(Sql, MyConection)
            Dim reader As NpgsqlDataReader
            reader = cmd.ExecuteReader()

            Dim lista As New List(Of String)

            While reader.Read()
                lista.Add(reader.Item(0))
            End While
            MyConection.Close()

            For Each numero As String In lista
                Console.WriteLine(numero.ToString())
            Next

        End Sub

    End Module







