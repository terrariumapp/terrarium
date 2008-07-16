
<Assembly: OrganismClass("Exercise1.MyAnimal")> 
<Assembly: AuthorInformation("Your Name", "someone@microsoft.com")> 

< _
    Carnivore(False), _
    MatureSize(36), _
    AnimalSkin(AnimalSkinFamily.Beetle), _
    MarkingColor(KnownColor.Red), _
    MaximumEnergyPoints(20), _
    EatingSpeedPointsAttribute(0), _
    AttackDamagePointsAttribute(12), _
    DefendDamagePointsAttribute(12), _
    MaximumSpeedPointsAttribute(16), _
    CamouflagePointsAttribute(10), _
    EyesightPointsAttribute(20) _
> _
Public Class MyAnimal : Inherits Animal

    Dim targetPlant As PlantState = Nothing    ' The current plant we're going after
    Protected Overloads Overrides Sub Initialize()

        ' TODO: Add Initialization logic here
        AddHandler Idle, AddressOf MyAnimal_Idle
        AddHandler Load, AddressOf MyAnimal_Load

    End Sub

    ' First event fired on an organism each turn 
    Sub MyAnimal_Load(ByVal sender As Object, ByVal e As LoadEventArgs)
        Try
            If Not (targetPlant Is Nothing) Then
                ' See if our target plant still exists (it may have died) 
                ' LookFor returns null if it isn't found 
                targetPlant = CType(LookFor(targetPlant), PlantState)
                If (targetPlant Is Nothing) Then

                    ' WriteTrace is the best way to debug your creatures. 
                    WriteTrace("Target plant disappeared.")
                End If
            End If
        Catch exc As Exception
            WriteTrace(exc.ToString())
        End Try
    End Sub

    ' Fired after all other events are fired during a turn 
    Sub MyAnimal_Idle(ByVal sender As Object, ByVal e As IdleEventArgs)
        Try
            ' Reproduce as often as possible 
            If (CanReproduce) Then
                BeginReproduction(Nothing)
            End If

            ' If we can eat and we have a target plant, eat 
            If (CanEat) Then
                WriteTrace("Hungry.")
                If Not (IsEating) Then
                    WriteTrace("Not eating: Have target plant?")
                    If Not (targetPlant Is Nothing) Then
                        WriteTrace("Yes, Have target plant already.")
                        If (WithinEatingRange(targetPlant)) Then
                            WriteTrace("Within Range, Start eating.")
                            BeginEating(targetPlant)
                            If (IsMoving) Then
                                WriteTrace("Stop while eating.")
                                StopMoving()
                            End If
                        Else
                            If Not (IsMoving) Then
                                WriteTrace("Move to Target Plant")
                                BeginMoving(New MovementVector(targetPlant.Position, 2))
                            End If
                        End If
                    Else
                        WriteTrace("Don't have target plant.")
                        If Not (ScanForTargetPlant()) Then
                            If Not (IsMoving) Then
                                WriteTrace("No plant found, so pick a random point and move there")
                                Dim RandomX As Integer = OrganismRandom.Next(0, WorldWidth - 1)
                                Dim RandomY As Integer = OrganismRandom.Next(0, WorldHeight - 1)
                                BeginMoving(New MovementVector(New Point(RandomX, RandomY), 2))
                            Else
                                WriteTrace("Moving and Looking...")
                            End If
                        End If
                    End If
                Else
                    WriteTrace("Eating.")
                    If (IsMoving) Then
                        WriteTrace("Stop moving while eating.")
                        StopMoving()
                    End If
                End If
            Else
                WriteTrace("Full: do nothing.")
                If (IsMoving) Then
                    StopMoving()
                End If
            End If
        Catch exc As Exception
            WriteTrace(exc.ToString())
        End Try
    End Sub

    ' Looks for target plants, and starts moving towards the first one it finds 
    Function ScanForTargetPlant() As Boolean
        Try
            Dim foundCreatures As System.Collections.ArrayList = Scan()
            If (foundCreatures.Count > 0) Then
                Dim orgState As OrganismState
                ' Always move after closest plant or defend closest creature if there is one 
                For Each orgState In foundCreatures
                    If (TypeOf orgState Is PlantState) Then
                        targetPlant = CType(orgState, PlantState)
                        BeginMoving(New MovementVector(orgState.Position, 2))
                        Return True
                    End If
                Next
            End If
        Catch exc As Exception
            WriteTrace(exc.ToString())
        End Try
        Return False
    End Function

    Public Overloads Overrides Sub SerializeAnimal(ByVal m As MemoryStream)

        ' TODO: Add Serialization logic here

    End Sub

    Public Overloads Overrides Sub DeserializeAnimal(ByVal m As MemoryStream)

        ' TODO: Add Deserialization logic here

    End Sub

End Class

