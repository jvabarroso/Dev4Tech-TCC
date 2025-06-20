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
    fontFamily: fonts.text,
    color: theme.text,
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
    color: theme.text3,
    alignSelf:"flex-start",
    paddingHorizontal:10,
  },
  textoanexo: {
    fontSize: 14,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: '#3288D7',
  },
  textosanexo:{
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingHorizontal: 10,
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
  inputinstrucoes: {
    width:"90%",
    height: 160,
    fontSize: 16,
    borderRadius: 6,
    borderWidth: 1,
    borderBottomColor: '#D6D3D1',
    backgroundColor: theme.inputBackground,
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
    alignSelf:"flex-start",
    backgroundColor: theme.inputBackground,
    borderRadius:10,
    borderWidth: 1,
    borderBottom: 0.10,
    borderColor: theme.border,
    padding:10,
  },
  containerequipes: {
    borderRadius: 10,
    padding: 10,
    marginBottom: 20,
    flexDirection: 'row',
    alignItems: 'center',
    width: '90%',
    alignSelf: 'center', 
    marginLeft: '5%', 
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
    color: theme.text3,
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