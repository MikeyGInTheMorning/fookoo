<script>
    
</script>

<div class="saying-container">
    <div class="header">
        This is the header
    </div>
    <div class="form">
        This is the form
        <button>help</button>
    </div>
    <div class="footer">
        this is the footer
    </div>
</div>

<style>
  .saying-container {
    position: absolute;

    top: 0;
    right: 0;
    bottom: 0;
    left: 0;

    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;

    background-color: rgb(145, 145, 145);
    color:hsl(85%, 20%, 10%);
  }

  @font-face {
    font-family: 'Pacifico';
    src: url('./Pacifico-Regular.ttf');
  }

  .saying {
    font: 400 100px/1.3 'Pacifico', Helvetica, sans-serif;
    font-size: 400%;
    font-style: italic;
    text-align: center;
  }
</style>
