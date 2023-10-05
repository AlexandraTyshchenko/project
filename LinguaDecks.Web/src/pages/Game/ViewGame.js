import './ViewGame.css';
import axios from 'axios';
import { Link } from 'react-router-dom';

class Progress{
  constructor(userId, deckId){
    this.userId = userId;
    this.deckId = deckId;
    this.answers = [];
  }
  
  SetCardProgress(cardID, answer) {
    const index = this.answers.findIndex((item) => item.CardId === cardID);
    if(index !==-1){
      this.answers[index] = {CardId: cardID, IsKnown: answer};
    }
    else{
      this.answers.push({CardId: cardID, IsKnown: answer});
    }
  }
}

function ViewGame(props){
  const deck = props.Deck;
  const userId = props.UserId;
  const cards = props.Deck.cards;

  var progress = new Progress(userId, deck.deckId);

  const api_URL = process.env.REACT_APP_API_URL;

  async function saveProgress(){
    await axios.post(`${api_URL}/api/decks/${deck.deckId}/progress`,{
      DeckId: progress.deckId,
      UserId: progress.userId,
      Answers: progress.answers
    });
  }
  
  function setCarousel(cards) {
    const carouselItems = cards.map((card, index) => (
      <div key={`${index}`} className={`carousel-item ${index === 0 ? 'active' : ''} bg-light position-relative rounded`}>
        <div flip="false" key-index = {`${index}`} className={`text-center position-absolute top-50 start-50 translate-middle fs-1`} >{card.primaryText}</div>
      </div>
    ));
  
    return (
      <div className="carousel-inner">
        {carouselItems}
      </div>
    );
  }

  function calculateProgress(){
    var positiveCount =0;
    var negativeCount =0;
    var unreachedCount=0;

    progress.answers.forEach((answer)=>{
      if(answer.IsKnown === true)
        positiveCount++;
      else
        negativeCount++;
    });

    unreachedCount = deck.cards.length - (positiveCount + negativeCount);
    var result = {positive: positiveCount, negative: negativeCount, unreached: unreachedCount, total: deck.cards.length};
    return(result);
  }

  function displayProgress(){
    var progress = calculateProgress();

    var percent = (progress.positive/progress.total) * 100;
    const successNode = document.querySelector('#positive-bar');
    successNode.style.width=`${percent}%`;
    successNode.childNodes[0].textContent = percent.toFixed(2) + ' %';

    percent = (progress.negative/progress.total) * 100;
    const dangerNode = document.querySelector('#negative-bar');
    dangerNode.style.width=`${percent}%`;
    dangerNode.childNodes[0].textContent = percent.toFixed(2) + ' %';

    percent = (progress.unreached/progress.total) * 100;
    const unreachedNode = document.querySelector('#unreached-bar');
    unreachedNode.style.width=`${percent}%`;
    unreachedNode.classList.remove('w-100');
    unreachedNode.childNodes[0].textContent = percent.toFixed(2) + ' %';
  }

  function knowAnswer(){
    answer("true");
  }

  function dontKnowAnswer(){
    answer("false");
  }

  function answer(isKnown){
    const node = document.querySelector('.carousel-item.active.position-relative.rounded');

    if(isKnown === "true"){
      node.classList.remove('bg-light','bg-danger-subtle');
      node.classList.add('bg-success-subtle');

      let cardIndex = node.childNodes[0].getAttribute('key-index');
      let cardId = deck.cards[cardIndex].id;
      progress.SetCardProgress(cardId,true);
    }
    else{
      node.classList.remove('bg-light','bg-success-subtle');
      node.classList.add('bg-danger-subtle');

      let cardIndex = node.childNodes[0].getAttribute('key-index');
      let cardId = deck.cards[cardIndex].id;
      progress.SetCardProgress(cardId,false);
    }
    
    displayProgress();


  }

  function flipCard(){
    const parentNode = document.querySelector('.carousel-item.active.position-relative.rounded');
    const element = parentNode.childNodes[0];
    const keyIndex = element.getAttribute('key-index');

    if(element.getAttribute('flip') === "false"){
      element.setAttribute('flip', true);
      element.textContent = deck.cards[keyIndex].secondaryText;
    }
    else{
      element.setAttribute('flip', false);
      element.textContent = deck.cards[keyIndex].primaryText;
    }
  }
  
  function resetFlip(){
    const parentNode = document.querySelector('.carousel-item.active.position-relative.rounded');
    const element = parentNode.childNodes[0];
    const keyIndex = element.getAttribute('key-index');
    if(element.getAttribute('flip') === "true"){
      element.setAttribute('flip', false);
      element.textContent = deck.cards[keyIndex].primaryText;
    }
  }

  return(
    <div className="game bg-primary-subtle bg-gradient container">
      <div className="row m-2 mt-5">
        <Link onClick={saveProgress} to={`/decks/${deck.deckId}`} key={deck.deckId}>
          <div className='col-auto btn btn-danger'>
            Exit
          </div>
         </Link>      
        <div className="col text-center">{deck.deckName}</div>
        <div className="col-auto">Cards: {deck.cards.length}</div>
      </div>

      <div id = "cards-carousel" className="my-3">
        <div id="carouselExample" class="carousel slide row align-items-center" data-wrap="false">
          <div className="col-3">
            <button class="btn btn-primary carousel-control-prev" type="button" data-bs-target="#carouselExample" data-bs-slide="prev" onClick={resetFlip}>
              <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            </button>
          </div>          

          <div className='col-6'>
            {setCarousel(cards)}            
          </div>
          
          <div className="col-3">
            <button class="btn btn-primary carousel-control-next col-3" type="button" data-bs-target="#carouselExample" data-bs-slide="next" onClick={resetFlip}>
              <span class="carousel-control-next-icon" ></span>
            </button>
          </div>          
        </div>

        <div className='row justify-content-center'>
          <div id = "answer-zone" className='col-6'>
            <div className='row justify-content-around my-3 '>
              <div className='col text-center '>
                <div id="positive-answer" className='btn btn-success h-100' onClick={knowAnswer}>Know</div>
              </div>
              <div className='col text-center '>
                <div className='btn btn-primary h-100' onClick={flipCard}>Flip Card</div>
              </div>
              <div className='col text-center '>
                <div id = "negative-answer" className='btn btn-danger h-100' onClick={dontKnowAnswer}>Don't Know</div>
              </div>
            </div>
          </div>
        </div>
        
        <div class="d-flex justify-content-center">
          <div class="col-6 progress-stacked">
            <div id='positive-bar' class="progress" role="progressbar" aria-label="Positive answers" aria-valuenow="50" aria-valuemin="0" aria-valuemax="200">
              <div class="progress-bar bg-success"></div>
            </div>
            <div id='unreached-bar' class="progress w-100" role="progressbar" aria-label="Unreached Cards" aria-valuenow="100" aria-valuemin="0" aria-valuemax="200">
              <div class="progress-bar "></div>
            </div>
            <div id = 'negative-bar' class="progress " role="progressbar" aria-label="Positive answers" aria-valuenow="50" aria-valuemin="0" aria-valuemax="200">
              <div class="progress-bar bg-danger"></div>
            </div>            
          </div>          
        </div> 
      </div>
    </div>
  );
}

export default ViewGame;