import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const getStyles = (theme) => StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: theme.background
  },
  scrollContent: {
    padding: 16,
  },
  titulo: {
    fontSize: 25,
    color:theme.text,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    padding: 10,
    alignSelf:"flex-start",
    flex: 1,
  },
  areaInput:{
    width: '100%',
    alignItems: 'center', 
    paddingVertical:10,
    paddingHorizontal: 20,
  },
  texto: {
    fontSize: 18,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color:theme.text3,
    alignSelf:"flex-start",
    paddingHorizontal:10,
  },
  textoanexo: {
    fontSize: 14,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: '#3288D7',
  },
  input: {
    width:"90%",
    borderRadius: 6,
    borderWidth: 1,
    borderBottomColor: theme.border,
    backgroundColor: theme.inputBackground,
    paddingVertical:8,    
    paddingHorizontal:20,
    marginBottom: 10,
    marginTop: 15,
  },
  inputequipe: {
    flex: 1,
    width:"100%",
    borderRadius: 6,
    borderWidth: 1,
    borderBottomColor: theme.border,
    backgroundColor: theme.inputBackground,
    paddingVertical:8,    
    paddingHorizontal:20,
    marginBottom: 10,
    marginTop: 15,
  },
  inputadicionar: {
    width:"15%",
    borderRadius: 6,
    borderWidth: 1,
    borderBottomColor: '#000000',
    backgroundColor: '#0C21C1',
    paddingVertical:8,    
    paddingHorizontal:20,
    marginBottom: 10,
    marginTop: 15,
    alignItems:"center",
    justifyContent:"center"
  },
  inputfuncionario: {
    width:"35%",
    marginBottom: 10,
    marginTop: 15,
    alignSelf:"flex-end",
    paddingHorizontal:10,
  },

  linha:{
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  botaoanexo:{
    alignContent:"flex-start",
    backgroundColor:"#ffffff",
    borderRadius:30,
    borderBottom: 0.1,
    borderBottomColor: '#000',
    padding:10,
    width: 160,
    height: 40,
  },
  containerequipes: {
    borderRadius: 10,
    padding: 10,
    marginBottom: 20,
    flexDirection: 'row',
    alignItems: 'center',
    width: '90%',
    alignSelf: 'center', 
  },
  imag: {
    width: 35,
    height: 35,
    marginLeft: 10,
  },
  textos: {
    marginLeft: 15,
    flex: 1,
  },
  textolistatitulo: {
    color: theme.text,
    fontSize: 15,
    fontWeight: 'bold',
    fontFamily: fonts.text,
  },
  textolistacargo: {
    color: theme.text,
    fontSize: 13,
    fontFamily: fonts.text,
  },
  textobotao: {
    fontSize: 14,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: theme.text,
  },
  textobotao2: {
    fontSize: 13,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: '#ffffff',
    alignItems:"center"
  },
  textobotao3: {
    fontSize: 13,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: '#D6D3D1',
    alignSelf:"flex-start",
  },
  botaocriar: {
    width: 200,
    backgroundColor: '#5BB14F',
    paddingVertical: 12,
    paddingHorizontal: 30,
    borderRadius: 10,
    alignItems: 'center',
    alignSelf: 'center',
    marginTop: 20,
  },
  textocriar: {
    color: '#fff',
    fontSize: 14,
    fontFamily: fonts.text,
    fontWeight: 'bold',
  },
})