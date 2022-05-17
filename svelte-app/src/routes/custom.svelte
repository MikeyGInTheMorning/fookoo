<script>
  import Form from '@svelteschool/svelte-forms';
  import { addSaying, cleanName } from './../lib/lib.js';

  let values;
  let url = '';

  const add = async () => {
    const saying = await addSaying(values.sentence);   
    const name = cleanName(values.name);

    if (saying) url = `http://localhost:5000/${name}/${saying.saying.hash}`;
    else url = 'ERROR'
  };
</script>

<div class="saying-container">
  <div class="header">This is the header</div>
  <div class="form">
    <Form bind:values>
      <input placeholder="Sendee" type="text" name="name" />
      <input placeholder="Type in your message" type="text" name="sentence" />
    </Form>
    <button on:click={add}>CLickme</button>
  </div>
  <div class="footer">
    this is the footer
    <input bind:value={url}>
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
    color: hsl(85%, 20%, 10%);
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
