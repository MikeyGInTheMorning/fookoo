<script>
  // import { page } from '$app/stores';
  import { firstLetterToUpper, getSaying } from './../lib/lib.js';

  export let data = '';
  export let sayingHash = '';

  let dataCleaned = firstLetterToUpper(data);
  let saying;
  let hue = 50;
  let fontHue = 0;

  getSaying(sayingHash).then((res) => (saying = res));

  setInterval(() => {
    hue++;
    fontHue = hue + 250;
  }, 50);
</script>

<title>Yeet</title>
<div
  style="color:hsl({fontHue}, 50%, 50%);background-color:hsl({hue}, 50%, 50%)"
  class="saying-container"
>
  {#if saying}
    <div class="saying">Hey {dataCleaned},</div>
    <div class="saying" style="text-decoration: underline;">{saying}</div>
  {/if}
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
  }

  @font-face {
    font-family: 'Pacifico';
    src: url('./Pacifico-Regular.ttf');
  }

  .saying {
    font: 400 100px/1.3 'Pacifico', Helvetica, sans-serif;
    font-size: 500%;
    font-style: italic;
    text-align: center;
  }
</style>
