// Early test

system Movement
	on msg::UpdateMessage
	select
		Position,
		Direction
	do
	{
		Position.vector := Position.vector + Direction.vector * msg.deltaTime;
	}

struct Position
{
	Vector::vec2
}