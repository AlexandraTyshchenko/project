import { useEffect, useState } from 'react';

function Pagination({ paginate, totalPages, currentPage }) {
  const [pages, setPages] = useState([1, 2, 3, 4, 5, 6, 7]);
  const [clickedPage, setClickedPage] = useState(1);

  function MoveNext() {
    const updatedPages = pages.map((page) => page + 1);
    setPages(updatedPages);
    console.log(totalPages);
  }

  useEffect(() => {
    setClickedPage(currentPage)

  }, [currentPage]); //для того якщо фільтрація була проведена на іншій сторінці щоб перенапрвляло на 1 сторінку

  function MoveBack() {
    const updatedPages = pages.map((page) => page - 1);
    setPages(updatedPages);
  }

  return (
    <div style={{ marginTop: '10px', marginBottom: '-10px' }}>
      <nav aria-label="Page navigation">
        <ul className="pagination justify-content-center">
          <li
            className="page-item"
            onClick={() => {
              if (pages[0] > 1) {
                MoveBack();
              }
            }}
          >
            <a className="page-link" href="#" aria-label="Previous">
              <span aria-hidden="true">&laquo;</span>
            </a>
          </li>

          {pages.map((page) => (
            <li className="page-item" key={page}>
              {page <= totalPages ? (
                <a
                  className={`page-link ${page === clickedPage ? 'active' : ''
                    }`}
                  onClick={() => {
                    paginate(page);
                    setClickedPage(page);
                  }}
                >
                  {page}
                </a>
              ) : null}

            </li>
          ))}
          <li
            className="page-item"
            onClick={() => {
              if (pages[pages.length - 1] < totalPages) {
                MoveNext();
              }
            }}
          >
            <a className="page-link" href="#" aria-label="Next">
              <span aria-hidden="true">&raquo;</span>
            </a>
          </li>
        </ul>
      </nav>
    </div>
  );
}

export default Pagination;
