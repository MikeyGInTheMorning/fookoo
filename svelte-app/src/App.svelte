<script>
  import { Router, Route } from 'svelte-routing';
  import Data from './routes/data.svelte';
  import Lost from './routes/lost.svelte';

  export let url = '';
</script>

<div class="section columns">
  <Router {url}>
    <main class="column">
      <div>
        <Route path="/:data" component={Data} />
        <Route path="**" component={Lost} />
      </div>
    </main>
  </Router>
</div>
