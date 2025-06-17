import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#ffffff",
  },
  scrollContent: {
    padding: 16,
  },
  titulo: {
    fontSize: 25,
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
    color: '#5e5e5e',
  },
  textoanexo: {
    fontSize: 14,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: '#3288D7',
  },
  input: {
    width:"90%",
    fontSize: 16,
    borderRadius: 6,
    borderWidth: 1,
    borderBottomColor: '#D6D3D1',
    backgroundColor: 'transparet',
    paddingVertical:8,    
    paddingHorizontal:20,
    marginBottom: 10,
    marginTop: 15,
  },
  inputinstrucoes: {
    width:"90%",
    height: 160,
    fontSize: 16,
    borderRadius: 6,
    borderWidth: 1,
    borderBottomColor: '#D6D3D1',
    backgroundColor: 'transparent',
    paddingVertical:8,    
    paddingHorizontal:20,
    marginBottom: 10,
    marginTop: 15,
    textAlignVertical: 'top'
  },
  linha:{
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop:10,
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
    color: '#000',
    fontSize: 15,
    fontWeight: 'bold',
    fontFamily: fonts.text,
  },
  textolistacargo: {
    color: '#000',
    fontSize: 13,
    fontFamily: fonts.text,
  },
  textobotao: {
    fontSize: 14,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: '#5e5e5e',
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