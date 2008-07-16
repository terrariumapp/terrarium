Imports System
Imports System.Drawing
Imports System.Collections
Imports System.IO
Imports OrganismBase

' Sample Carnivore
' Strategy: This animal stands still until it sees something tasty and then
' bites it.  If any dead animals are within range it eats them.

' The following Assembly attributes must be applied to each Organism Assembly
' The class that derives from Animal

' Provide Author Information
' Author Name, Email


Namespace VBCreatures
    ' Carnivore = True, It's a Carnivore
    ' MatureSize = 30, Must be between 24 and 48, 24 means faster
    ' reproduction while 48 would give more defense and attack power
    '
    ' AnimalSkin = AnimalSkinFamilyEnum.Scorpion, you can be a Beetle
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
    ' MaximumEnergyPoints = 0, No need to store energy.
    ' EatingSpeedPoints = 0, No need to eat fast.
    ' AttackDamagePoints = 52, Needs to deal some good damage
    ' DefendDamagePoints = 0, Attacks, so no need for defense
    ' MaximumSpeedPoints = 28, Moves somewhat fast to catch prey
    ' CamouflagePoints = 0, Doesn't mind if noticed
    ' EyesightPoints = 20, Needs to see prey and food
    < _
        CarnivoreAttribute(True), _
        MatureSize(30), _
        AnimalSkin(AnimalSkinFamily.Beetle), _
        MarkingColor(KnownColor.Red), _
        MaximumEnergyPoints(0), _
        EatingSpeedPoints(0), _
        AttackDamagePoints(52), _
        DefendDamagePoints(0), _
        MaximumSpeedPoints(28), _
        CamouflagePoints(0), _
        EyesightPoints(20) _
    > _
    Public Class SimpleCarnivore : Inherits Animal
        Dim targetAnimal As AnimalState

        Protected Overloads Overrides Sub Initialize()
            AddHandler Load, New LoadEventHandler(AddressOf LoadEvent)
            AddHandler Idle, New IdleEventHandler(AddressOf IdleEvent)
        End Sub

        ' First event fired on an organism each turn
        Private Sub LoadEvent(ByVal Sender As Object, ByVal e As LoadEventArgs)
            Try
                If Not targetAnimal Is Nothing Then
                    ' See if our target animal still exists (it may have died)
                    ' LookFor returns null if it isn't found
                    targetAnimal = CType(LookFor(targetAnimal), AnimalState)
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

                ' If we are already doing something, then we don't
                ' need to do something else.  Lets leave the
                ' function.
                If IsAttacking Or IsMoving Or IsEating Then
                    Return
                End If

                ' Try to find a new target if we need one.
                If targetAnimal Is Nothing Then
                    FindNewTarget()
                End If

                ' If we have a target animal then lets check him out
                If Not targetAnimal Is Nothing Then
                    ' If the target is alive, then we need to kill it
                    ' else we can immediately eat it.
                    If targetAnimal.IsAlive Then
                        ' If we are within attacking range, then
                        ' lets eat the creature.  Else we'll
                        ' try to move into range
                        If WithinAttackingRange(targetAnimal) Then
                            BeginAttacking(targetAnimal)
                        Else
                            MoveToTarget()
                        End If
                    Else
                        ' If the creature is dead then we can try to eat it.
                        ' If we are within eating range then we'll try to
                        ' eat the creature, else we'll move towards it.
                        If WithinEatingRange(targetAnimal) Then
                            If CanEat Then
                                BeginEating(targetAnimal)
                            End If
                        Else
                            MoveToTarget()
                        End If
                    End If
                Else
                    ' If we stop moving we conserve energy.  Sometimes
                    ' this works better than running around.
                    StopMoving()
                End If
            Catch exc As Exception
                WriteTrace(exc.ToString())
            End Try
        End Sub


        Private Sub FindNewTarget()
            Try
                Dim foundAnimals As ArrayList = Scan()

                ' We should see at least a couple of animals.
                If foundAnimals.Count > 0 Then
                    Dim orgState As OrganismState
                    For Each orgState In foundAnimals
                        ' We are looking for any organism that is an animal
                        ' and that isn't of our species so we can eat them.
                        If TypeOf orgState Is AnimalState And Not IsMySpecies(orgState) Then
                            targetAnimal = CType(orgState, AnimalState)
                        End If
                    Next
                End If
            Catch exc As Exception
                WriteTrace(exc.ToString())
            End Try
        End Sub

        ' Function used to move towards our prey or meal
        Private Sub MoveToTarget()
            Try
                ' Make sure we aren't moving towards a null target
                If targetAnimal Is Nothing Then
                    Return
                End If

                ' Follow our target as quickly as we can
                BeginMoving(New MovementVector(targetAnimal.Position, Species.MaximumSpeed))
            Catch exc As Exception
                WriteTrace(exc.ToString())
            End Try
        End Sub

        ' This gets called whenever your animal is being saved -- either the game is closing
        ' or you are being teleported.  Store anything you want to remember when you are
        ' instantiated again in the stream.
        Public Overloads Overrides Sub SerializeAnimal(ByVal m As MemoryStream)
        End Sub

        ' This gets called when you are instantiated again after being saved.  You get a 
        ' chance to pull out any information that you put into the stream when you were saved
        Public Overloads Overrides Sub DeserializeAnimal(ByVal m As MemoryStream)
        End Sub
    End Class
End Namespace

