import { StyleSheet } from "react-native";
import fonts from "../../styles/fonts";



export const getStyles = (theme) => StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: theme.background,
  },
  scrollContent: {
    padding: 16,
  },
  nav:{
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingVertical: 15,
  },
  botaodevoltar:{
    width:30,
    height:30,
    marginLeft: 1,
    color:"#0C21C1",
    fontWeight: '300',
    flexDirection: 'row',
    marginTop:3
  },
  logo:{
    fontSize: 18,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: theme.text,
    textAlign: 'center',
    flex: 1,
  },
  titulo: {
    fontSize: 30,
    fontFamily: fonts.text,
    color: theme.text,
    fontWeight: 'bold',
    paddingVertical:15,
    marginLeft:20,
  },
  titulo2: {
    fontSize: 15,
    fontFamily: fonts.text,
    color: theme.text,
    fontWeight: 'bold',
    padding:15,
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
    marginBottom: 10,
    color: theme.text,
  },
  flat: {
    flex: 1,
  },
  containertarefas: {
    backgroundColor: theme.inputBackground,
    borderRadius: 10,
    padding: 10,
    marginBottom: 20,
    flexDirection: 'row',
    alignItems: 'center',
  },
  containerfuncionario: {
    backgroundColor: theme.inputBackground,
    borderRadius: 10,
    padding: 10,
    marginBottom: 10,
    flexDirection: 'row',
    alignItems: 'center',
    alignSelf:"center",
    marginRight:10,
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
    color: theme.text,
    fontSize: 18,
    fontWeight: 'bold',
    fontFamily: fonts.text,
  },
  textolistacargo: {
    color: theme.text,
    fontSize: 15,
    fontFamily: fonts.text,
  },
  textofuncionario: {
    color: theme.text,
    fontSize: 28,
    fontWeight: 'bold',
    fontFamily: fonts.text,
  },
  textofuncionariocargo: {
    color: theme.text,
    fontSize: 19,
    fontWeight: '300',
    fontFamily: fonts.text,
  },
  pontuacao: {
    fontSize: 16,
    fontWeight: 'bold',
    color: theme.text,
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
    color:"#0C21C1",
    fontWeight: '300',
    width:60,
  },
  areaInput:{
    width: '100%',
    alignItems: 'flex-start',
    marginLeft:20,
    paddingVertical:10,
  },
  texto: {
    fontSize: 18,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: theme.text,
    marginBottom: 5,
  },
  input: {
    width:"90%",
    fontSize: 16,
    borderRadius: 6,
    borderWidth: 1,
    borderBottomColor: '#D6D3D1',
    backgroundColor: theme.inputBackground,
    paddingVertical:8,    
    paddingHorizontal:20,
    marginBottom: 10,
    marginTop: 15,
  },
  botaoeditar: {
    backgroundColor: '#1C58F2',
    paddingVertical: 10,
    paddingHorizontal: 30,
    borderRadius: 10,
    alignItems: 'center',
    alignSelf: 'center',
    marginTop: 20,
  },
  botaosair: {
    backgroundColor: '#F21C1C',
    paddingVertical: 10,
    paddingHorizontal: 30,
    borderRadius: 10,
    alignItems: 'flex-start',
    alignSelf: 'flex-start',
    marginTop: 20,
  },
  textoeditar: {
    color: "#ffff",
    fontSize: 16,
    fontFamily: fonts.text,
    fontWeight: 'bold',
  },
  inputfuncionario: {
    width:"35%",
    marginBottom: 10,
    marginTop: 15,
    alignSelf:"flex-start",
    paddingHorizontal:10,
  },
  textobotao3: {
    fontSize: 13,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: '#D6D3D1',
    alignSelf:"flex-start",
    width: 150,
  },
  textomodo: {
    color: theme.text,
    fontSize: 15,
    fontFamily: fonts.text,
  },
  botaomodo: {
    backgroundColor: theme.background,
    paddingHorizontal: 30,
    borderRadius: 10,
    alignItems: 'center',
    alignSelf: 'center',
  },  
});