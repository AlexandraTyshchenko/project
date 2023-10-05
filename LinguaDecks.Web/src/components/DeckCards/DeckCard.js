import ProgressBar from './ProgressBar';
import { Link } from 'react-router-dom';

function DeckCard({ deckId, deckName, progress, icon }) {
  return (
    <div className="card text-center">
      <div className="card-body">
        <Link to={`/decks/${deckId}`} key={deckId}>
          <div className="card-title f-5 btn">{deckName}</div>
        </Link>

        <div className='deck-img'>
          <img src={icon} alt="Deck Icon" className="mx-auto d-block"/>
        </div>

        <div className="card-footer">
          <div className='d-flex'>
            <ProgressBar progress={progress} />
          </div>
        </div>
      </div>
    </div>
  );
}

export default DeckCard;