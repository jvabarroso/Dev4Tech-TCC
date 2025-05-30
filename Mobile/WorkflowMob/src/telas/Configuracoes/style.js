import { StyleSheet } from "react-native";
import fonts from "../../styles/fonts";



export const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#ffffff",
  },
  scrollView: {
    flex: 1,
  },
  containerConteudo: {
    paddingHorizontal: 20,
    paddingTop: 15,
    paddingBottom: 40,
  },
  nav:{
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingVertical: 15,
    marginBottom: 10,
  },
  botaodevoltar:{
    width:30,
    height:30,
    marginLeft: 1,
    flexDirection: 'row',
    marginTop:3
  },
  logo:{
    fontSize: 18,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    textAlign: 'center',
    flex: 1,
  },
  titulo: {
    fontSize: 30,
    fontFamily: fonts.text,
    color: '#000',
    fontWeight: 'bold',
    paddingVertical:15,
    marginLeft:20,
  },
  titulo2: {
    fontSize: 15,
    fontFamily: fonts.text,
    color: '#000',
    fontWeight: 'bold',
    marginTop: '5%',
    marginBottom: "8%",
  },
  navinput: {
    width: '100%',
    padding: 10,
    fontSize: 17,
    fontFamily: fonts.text,
    backgroundColor: '#1C58F2',
    borderRadius: 10,
    borderBottomWidth: 0.1,
    borderBottomColor: '#000',
    marginBottom: 15,
    color: '#fff',
  },
  flat: {
    flex: 1,
  },
  containertarefas: {
    backgroundColor: '#F5F7FC',
    borderRadius: 10,
    padding: 10,
    marginBottom: 20,
    flexDirection: 'row',
    alignItems: 'center',
  },
  containerfuncionario: {
    backgroundColor: '#ffffff',
    borderRadius: 10,
    padding: 10,
    marginBottom: 20,
    flexDirection: 'row',
    alignItems: 'center',
  },
  imag: {
    width: 45,
    height: 45,
    marginLeft: 10,
  },
  imagemfuncionario: {
    width: 90,
    height: 90,
    marginLeft: 10,
  },
  textos: {
    marginLeft: 15,
    flex: 1,
  },
  textolistatitulo: {
    color: '#000',
    fontSize: 18,
    fontWeight: 'bold',
    fontFamily: fonts.text,
  },
  textolistacargo: {
    color: '#000',
    fontSize: 15,
    fontFamily: fonts.text,
  },
  textofuncionario: {
    color: '#000',
    fontSize: 28,
    fontWeight: 'bold',
    fontFamily: fonts.text,
  },
  textofuncionariocargo: {
    color: '#000',
    fontSize: 19,
    fontFamily: fonts.text,
  },
  pontuacao: {
    fontSize: 16,
    fontWeight: 'bold',
    fontFamily: fonts.text,
    marginBottom: 10,
    marginTop:3
  },
  pontuacaotext: {
    fontSize: 16,
    fontWeight: 'bold',
    fontFamily: fonts.text,
    marginBottom: 10,
    color:"#5BB14F"
  },
  linha:{
    flexDirection: 'row',
    justifyContent: 'space-between',
    width: '100%',
    marginTop: 10,
    paddingHorizontal: 6,
    marginBottom:28
  },
  voltar:{
    fontSize: 18,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    width:60,
  },
  areaInput:{
    width: '100%',
    alignItems: 'flex-start',
    marginBottom: "10%",
    marginLeft:20,
  },
})