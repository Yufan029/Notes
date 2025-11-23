<template>
  Username: <input type="text" v-model="state.loginData.username" placeholder="Username" />
  Password: <input type="password" v-model="state.loginData.password" placeholder="Password" />
  <input type="submit" value="Login" @click="loginSubmit" />

  <ul>
    <li v-for="p in state.processInfos" :key="p.id">
      {{ p.id }} - {{ p.name }} - {{ (p.workingSet64 / 1024).toFixed(0) }}K
    </li>
  </ul>
</template>

<script>
import axios from 'axios'
import {reactive, onMounted} from 'vue'

export default { 
  name: 'Login',
  setup() {
    const state = reactive({loginData:{}, processInfos:[]});

    const loginSubmit = async () => {
      const payload = state.loginData;
      const resp = await axios.post('https://localhost:7228/login/login', payload);
      const data = resp.data;
      if (!data.success)
      {
        alert('Login failed');
        return;
      }
      
      state.processInfos = data.processInfos;
    }

    return {state, loginSubmit}
  }
 }
</script>
