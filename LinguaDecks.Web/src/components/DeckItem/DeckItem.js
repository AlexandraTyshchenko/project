import './deck.css';

function DeckItem(props) {
  return (
    <div className="container ">
      <div className="row justify-content-md-center deckitem text-uppercase fw-bold">
        <div className="col mr-3">{props.title} </div>
        <div className="col mr-3">{props.cardsnumber} cards</div>
        <div className="col mr-3">{props.category} </div>
        <div className="col mr-3 star">
          {props.rate}
          <span className="star-icon">&#9733;</span>
        </div>
        <div className="col">{props.author}</div>
      </div>
    </div>
  );
}

export default DeckItem;
