const form = document.getElementById('ratingForm');

form.onsubmit = function(e) {
  e.preventDefault();
  const valueStars = document.querySelector('input[name="rating"]:checked').value;
  showThankyou(valueStars);
}

function showThankyou(val) {
  const starText = val > 1 ? 'stars' : 'star';
  const panel = document.querySelector('.panel');
  panel.innerHTML = `
    <span class="fa-heart"></span>
    <h1>Thank you!</h1>
    <br>
    <strong>Feedback: ${val} ${starText}</strong>
    <p>We'll use your feedback to improve our support.</p>
  `;
}

function handleChange() {
  const inputRatings = document.querySelectorAll('input[name="rating"]');
  const submitBtn = document.querySelector('input[type="submit"]');
  
  inputRatings.forEach(input => {
    input.addEventListener('change', () => {
      if (input.checked === true) {
        submitBtn.disabled = false;
      }
    })
  })
}

handleChange();