Create Procedure spDeleteActor
@ActorID int
as
Begin
  Delete from Actor where ActorID = @ActorID
End