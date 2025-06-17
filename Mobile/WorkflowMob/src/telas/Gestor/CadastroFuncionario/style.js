import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: "#ffffff"
  },
  scroll: {
    flex: 1,
    width: '100%',
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
    alignItems: 'flex-start',
    marginLeft:20,
    paddingVertical:10,
    paddingHorizontal: 20,
  },
  texto: {
    fontSize: 18,
    fontFamily: fonts.text,
    fontWeight: 'bold',
    color: '#5e5e5e',
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
})