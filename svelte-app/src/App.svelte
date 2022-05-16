<script>
  import { Router, Route } from 'svelte-routing';
  import Data from './routes/data.svelte';
  import Lost from './routes/lost.svelte';
  import Custom from './routes/custom.svelte';

  export let url = '';
</script>

<div class="section columns">
  <Router {url}>
    <main class="column">
      <div>
        <Route path="/custom" component={Custom} />
        <Route path="/:data" component={Data} />
        <Route path="**" component={Lost} />
      </div>
    </main>
  </Router>
</div>
