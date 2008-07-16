Imports System
Imports System.Drawing
Imports System.Collections
Imports System.IO
Imports OrganismBase

' Sample Herbivore
' Strategy: This animal moves until it finds a food source and then stays there

' The following Assembly attributes must be applied to each Organism Assembly
' The class that derives from Animal
 
' Provide Author Information
' Author Name, Email
 

Namespace VBCreatures
    ' Carnivore = False, It's a Herbivore
    ' MatureSize = 26, Must be between 24 and 48, 24 means faster
    ' reproduction while 48 would give more defense and attack power
    '
    ' AnimalSkin = AnimalSkinFamilyEnum.Beetle, you can be a Beetle
    ' an Ant, a Scorpion, an Inchworm, or a Spider
    ' MarkingColor = KnownColor.Red, you can choose to mark your
    ' creature with a color.  This does not affect appearance in
    ' the game.
    '
    ' Point Based Attributes
    ' You get 100 points to distribute among these attributes to define
    ' what your organism can do.  Choose them based on the strategy
    ' your organism will use.  This organism hides and has good eyesight
    ' to find plants.
    '
    ' MaximumEnergyPoints = 0, Sits next to plants, doesn't need an energy store
    ' EatingSpeedPoints = 0, Sits next to plants, doesn't need faster eating speed
    ' AttackDamagePoints = 0, Doesn't ever attack
    ' DefendDamagePoints = 0, Doesn't ever defend
    ' MaximumSpeedPoints = 0, Doesn't need to move quickly
    ' CamouflagePoints = 50, Try to remain hidden
    ' EyesightPoints = 50, Need to find plants better
    < _
        Carnivore(False), _
        MatureSize(26), _
        AnimalSkin(AnimalSkinFamily.Beetle), _
        MarkingColor(KnownColor.Red), _
        MaximumEnergyPoints(0), _
        EatingSpeedPointsAttribute(0), _
        AttackDamagePointsAttribute(0), _
        DefendDamagePointsAttribute(0), _
        MaximumSpeedPointsAttribute(0), _
        CamouflagePointsAttribute(50), _
        EyesightPointsAttribute(50) _
    > _
    Public Class SimpleHerbivore : Inherits Animal
        ' Our Current Target Plant
        Dim targetPlant As PlantState = Nothing

        ' Set up any event handlers that we want when first initialized
        Protected Overloads Overrides Sub Initialize()
            AddHandler Load, New LoadEventHandler(AddressOf LoadEvent)
            AddHandler Idle, New IdleEventHandler(AddressOf IdleEvent)
        End Sub

        ' First event fired on an organism each turn
        Private Sub LoadEvent(ByVal Sender As Object, ByVal e As LoadEventArgs)
            Try
                If Not targetPlant Is Nothing Then
                    ' See if our target plant still exists (it may have died)
                    ' LookFor returns null if it isn't found
                    targetPlant = CType(LookFor(targetPlant), PlantState)
                End If
            Catch exc As Exception
                ' WriteTrace is useful in debugging creatures
                WriteTrace(exc.ToString())
            End Try
        End Sub

        ' Fired after all other events are fired during a turn
        Private Sub IdleEvent(ByVal Sender As Object, ByVal e As IdleEventArgs)
            Try
                ' Our Creature will reproduce as often as possible so
                ' every turn it checks CanReproduce to know when it
                ' is capable.  If so we call BeginReproduction with
                ' a null Dna parameter to being reproduction.
                If CanReproduce Then
                    BeginReproduction(Nothing)
                End If

                ' Check to see if we are capable of eating
                ' If we are then try to eat or find food,
                ' else we'll just stop moving.
                If CanEat And Not IsEating Then
                    ' If we have a Target Plant we can try
                    ' to either eat it, or move towards it.
                    ' else we'll move to a random vector
                    ' in search of a plant.
                    If Not targetPlant Is Nothing Then
                        ' If we are within range start eating
                        ' and stop moving.  Else we'll try
                        ' to move towards the plant.
                        If WithinEatingRange(targetPlant) Then
                            BeginEating(targetPlant)
                            If (IsMoving) Then
                                StopMoving()
                            End If
                        Else
                            If Not IsMoving Then
                                BeginMoving(New MovementVector(targetPlant.Position, 2))
                            End If
                        End If
                    Else
                        ' We'll try try to find a target plant
                        ' If we don't find one we'll move to 
                        ' a random vector
                        If Not ScanForTargetPlant() Then
                            If Not IsMoving Then
                                Dim RandomX As Integer = OrganismRandom.Next(0, WorldWidth - 1)
                                Dim RandomY As Integer = OrganismRandom.Next(0, WorldHeight - 1)
                                BeginMoving(New MovementVector(New Point(RandomX, RandomY), 2))
                            End If
                        End If
                    End If
                Else
                    ' Since we are Full or we are already eating
                    ' We should stop moving.
                    If (IsMoving) Then
                        StopMoving()
                    End If
                End If
            Catch exc As Exception
                WriteTrace(exc.ToString())
            End Try
        End Sub

        'Looks for target plants, and starts moving towards the first one it finds
        Private Function ScanForTargetPlant() As Boolean
            Try
                ' Find all Plants/Animals in range
                Dim foundAnimals As ArrayList = Scan()

                ' If we found some Plants/Animals lets try
                ' to weed out the plants.
                If foundAnimals.Count > 0 Then
                    Dim orgState As OrganismState

                    For Each orgState In foundAnimals
                        ' If we found a plant, set it as our target
                        ' then begin moving towards it.  Tell the
                        ' caller we have a target.
                        If TypeOf orgState Is PlantState Then
                            targetPlant = CType(orgState, PlantState)
                            BeginMoving(New MovementVector(orgState.Position, 2))
                            Return True
                        End If
                    Next
                End If
            Catch exc As Exception
                WriteTrace(exc.ToString())
            End Try

            ' Tell the caller we couldn't find a target
            Return False
        End Function

        ' This gets called whenever your animal is being saved -- either the game is closing
        ' or you are being teleported.  Store anything you want to remember when you are
        ' instantiated again in the stream.
        Public Overloads Overrides Sub SerializeAnimal(ByVal m As MemoryStream)
        End Sub

        'This gets called when you are instantiated again after being saved.  You get a 
        'chance to pull out any information that you put into the stream when you were saved
        Public Overloads Overrides Sub DeserializeAnimal(ByVal m As MemoryStream)
        End Sub
    End Class
End Namespace

