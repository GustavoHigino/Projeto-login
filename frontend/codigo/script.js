const slogin=document.getElementById("login");
const clogin=document.getElementById("criarlogin");
const btncriar=document.getElementById("btncriar");
const btnvoltar=document.getElementById("btnvoltar");
btncriar.addEventListener("click",()=>{
    clogin.classList.add("entrando");
    slogin.classList.add("saindo");
    setTimeout(()=>{
        
        clogin.classList.add("ativo")
        slogin.classList.remove("ativo","saindo")
        clogin.classList.remove("entrando");
    },500);
})
btnvoltar.addEventListener("click",()=>{
    slogin.classList.add("entrando");
    clogin.classList.add("saindo");

    setTimeout(()=>{
        slogin.classList.add("ativo")
        clogin.classList.remove("ativo","saindo")
        slogin.classList.remove("entrando");
    },500);
    
})
const btnregistro = window.document.getElementById("registro")
const mensagemn = document.getElementById("mensagemn")
const mensageme = document.getElementById("mensageme")
const mensagems = document.getElementById("mensagems")
const mensagemcs = document.getElementById("mensagemcs")
const mensagemfinal=document.getElementById("mensagemfeito")
const nome= document.getElementById("cnome")
const email= document.getElementById("cemail")
const senha= document.getElementById("csenha")
const csenha= document.getElementById("casenha")

btnregistro.addEventListener("click",async ()=>{
    let nomev=nome.value
    let emailv=email.value
    let senhav=senha.value
    let csenhav=csenha.value
    if(!(String(nomev).trim().length>4)){
        mensagemn.textContent="*nome invalido! minimo 5 caracteres"
        mensagemn.style.color="red"
    }
    else{
        mensagemn.textContent=""
    }
    if(!(String(emailv).trim().length>4 && emailv.includes("@"))){
        mensageme.textContent="*E-mail inválido minimo 5 caracteres"
        mensageme.style.color="red"
    }
    else{
        mensageme.textContent=""
    }
    if(!(String(senhav).trim().length>7)){
        mensagems.textContent="*senha invalida minimo 8 caracteres"
        mensagems.style.color="red"
    }
    else{
        mensagems.textContent=""
    }
    if(!(String(csenhav)==String(senhav))){
        mensagemcs.textContent="*senhas incompatíveis"
        mensagemcs.style.color="red"
    }
    else{
        mensagemcs.textContent=""
    }
    if(mensagemcs.textContent=="" && mensageme.textContent=="" && mensagemn.textContent=="" && mensagems.textContent==""){
        mensagemfinal.textContent="Verifique seu e-mail, e confirme para ter acesso a conta."
        mensagemfinal.style.color="green"
        let dados = {nome:nomev,email:emailv,senha:senhav}
        let resposta = await fetch("https://localhost:7178/login",{method:"POST",headers:{
            "Content-Type": "application/json"
        },body:JSON.stringify(dados)})

        
        console.log=(resposta)

        let texto = await resposta.text()
        console.log(texto)
    } 
})
const btnlogar=window.document.getElementById("entrar")
let lemail = window.document.getElementById("lemail")
let lsenha = window.document.getElementById("lsenha")
let mensagemlogin=window.document.getElementById("mensagemlogin")
btnlogar.addEventListener("click",async ()=>{
    let lemailv= lemail.value
    let lsenhav=lsenha.value
    let dados ={email:lemailv, senha:lsenhav}
    let resposta = await fetch("https://localhost:7178/login/verificar",{method:"POST",headers:{"Content-Type": "application/json"}
    ,body:JSON.stringify(dados)})
    if(resposta.ok){
        let usuario = await resposta.json()

        window.location.href="proxima.html"
    }
    if(resposta.status==403){
        mensagemlogin.innerText=""
        mensagemlogin.innerText="Valide seu e-mail"
        
    }
    if(resposta.status==401){
        mensagemlogin.innerText=""
        mensagemlogin.innerText="senha e e-mail incorretos ou invalidos"
    }
    
    
    
  
})

