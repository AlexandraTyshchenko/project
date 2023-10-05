function ProgressBar(props) {
  const progress = props.progress;
  var percentPositive = "0";
  var percentNegative = "0";
  var percentUnreached = "0";
  if(progress.total > 0){
    percentPositive = (progress.positive / progress.total) * 100 + "%";
    percentNegative = (progress.negative / progress.total) * 100 + "%";
    percentUnreached = (1 - (progress.positive + progress.negative) / progress.total) * 100 + "%" ;
  }
  else{
    percentUnreached = "100%";
  }
  
  console.log(percentPositive);
  console.log(percentNegative);
  console.log(percentUnreached);
   
  return (
    <div className="col progress-stacked">
      <div className="progress" role="progressbar" style={{width: `${percentPositive}`}} aria-label="Positive answers"  aria-valuemin="0" aria-valuemax="100">
        <div className="progress-bar bg-success" >{progress.positive}</div>
      </div>
      <div className="progress" role="progressbar" style={{width: `${percentUnreached}`}} aria-label="Unreached Cards"  aria-valuemin="0" aria-valuemax="100">
        <div className="progress-bar" >{progress.total - (progress.positive + progress.negative)}</div>
      </div>
      <div className="progress" role="progressbar" style={{width: `${percentNegative}`}} aria-label="Positive answers"   aria-valuemin="0" aria-valuemax="100">
        <div className="progress-bar bg-danger" >{progress.negative}</div>
      </div>            
    </div> 
  );
}

export default ProgressBar;