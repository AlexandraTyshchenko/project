const Footer = () => {
  const year = new Date().getFullYear();
  
  return(
    <footer id="footer" class = "bg-primary text-white">{`Copyright © ${year} `}
      <a class = "text-white" href="#">LinguaDecks.com</a> 
      <p>support: support@mail.com</p>
    </footer>
  );  
};
  
export default Footer;