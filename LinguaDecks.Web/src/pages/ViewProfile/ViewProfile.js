import defaultUserIcon from '../../assets/default_user_avatar.png';
import defaultDeckIcon from '../../assets/default_deck_logo.svg';
import './ViewProfile.css';
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";
import { Pie } from "react-chartjs-2";
import DeckCard from '../../components/DeckCards/DeckCard';

ChartJS.register(ArcElement, Tooltip, Legend);

function ViewProfile(props){
  const totalProgress = props.TotalProgress;
  const allProgresses = props.AllProgresses;
  const user = props.User;
  const deckIcons = props.DeckIcons;

  function totalDiagram(){
    if(totalProgress.total > 0){
      const data = {
        labels: ["Know", "Unanswered", "Don't Know" ],
        datasets: [{
          data: [totalProgress.positive, totalProgress.total - (totalProgress.positive + totalProgress.negative), totalProgress.negative],
          backgroundColor: [
            '#198754', // success
            '#0d6efd',  // primary
            '#dc3545', // danger
          ],
          hoverOffset: 15,
          borderColor: "transparent"
        }]
      };
      const options = {
        maintainAspectRatio: false,
        cutoutPercentage: 10,
        plugins: {
          legend: {
            position: 'right'
          }
        }
      };
      
      return (
        <div>
          <div className='text-center'>Total score</div>      
          <div id = "total-score" className='w-75'>
            <Pie data = {data} options = {options} />
          </div>
        </div>
    );
      
    }    
  }

  function progressCards(){
    const icon = defaultDeckIcon;// deckIcons.find(item => item.deckId === deckId).iconPath;
    var cardList =allProgresses.map(deck => (
      <div className='col-md-3 col-sm-6 my-3'>
        <DeckCard key={deck.deckId} deckId={deck.deckId} deckName={deck.deckName} progress={deck.progress} icon = {icon} />
      </div>
      
    ));

    return(
    <div className='row my-3 ms-4'>
      {cardList}
    </div>);
  }

  function userData(){
    
    return(
      <div id = "user-data" className="row ms-5 me-5 my-5 ">
        <div className="col-md-4 align-items-center">
          <div className='row'>
            <div id = "user-icon" class = "bg-light border border-primary-subtle border-3 rounded">
              <img src={defaultUserIcon} alt="Deck Icon" class="mx-auto d-block"  />
            </div>
          </div>
          
          <div className='mt-4'>            
            <div className="row align-items-center text-center ">
              <div className="bg-light">
                {user.name}
              </div>
            </div>

            <div className="row align-items-center text-center my-2">
              <div className="bg-light">
              {user.email}
              </div>
            </div>

            <div className="row align-items-center text-center">
              <div id = 'role' className="bg-light text-secondary">
                {user.role}
              </div>
            </div>            
          </div>
        </div>

        <div className='col-md-8'>
        {totalDiagram()}
        </div>

      </div>
    );
  }

  return(
    <div id = "content" class="container bg-primary-subtle bg-gradient">
      {userData()}  

      <div className='row-12 border border-primary my-1 ms-4' />

      {progressCards()}
    </div>
  );
}

export default ViewProfile;