
struct Vector
{
	x::single
	y::single
	
}

struct Position::IComponent
{
	vector::Vector
}

struct Direction::IComponent
{
	vector::Vector
}

struct UpdateMessage::Message
{
	float deltaTime;
}

system Movement
	on msg::UpdateMessage
	select
		Position,
		Direction
	do
	{
		Position.vector += Direction.vector * msg.deltaTime;
	}