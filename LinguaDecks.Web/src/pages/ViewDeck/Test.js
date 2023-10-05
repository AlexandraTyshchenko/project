import React, { useEffect, useState } from 'react';

function Component(props) {
  const elements = [];

  for (let i = 0; i < 10; i++) {
    elements.push(
      <input
        key={i}
        onChange={(event) => props.SetArray(i, event.target.value)}
      />
    );
  }
  return <div>{elements}</div>;
}

function Test() {
  const [array, setArray] = useState([]);

  function renderElement() {
    var newArray = Array.from(array);
    for (let i = 0; i < 10; i++) {
      newArray[i] = i;
    }
    setArray(newArray);
  }

  useEffect(() => {
    renderElement();
  }, []); 

  function SetArray(key, value) {
    var newArray = [...array];
    newArray[key] = value;
    setArray(newArray);
  }

  return (
    <div>
      <Component SetArray={SetArray} />

      {array.map((item, index) => (
        <h1 key={index}>{item}</h1>
      ))}
    </div>
  );
}

export default Test;
